using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows;
using AutoMapper;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.EggProduction.Commands;
using EggFarmSystem.Models;
using EggFarmSystem.Resources;
using EggFarmSystem.Services;

namespace EggFarmSystem.Client.Modules.EggProduction.ViewModels
{
    public class EggProductionEntryViewModel : ViewModelBase
    {
        private readonly IMessageBroker broker;
        private readonly IEggProductionService service;
        private readonly IConsumableUsageService usageService;

        public EggProductionEntryViewModel(IMessageBroker broker, IEggProductionService service,IConsumableUsageService usageService,
            IHenHouseService houseService, SaveEggProductionCommand saveCommand, CancelCommand cancelCommand,
            ShowEggProductionListCommand showListCommand)
        {
            this.broker = broker;
            this.service = service;
            this.usageService = usageService;
            ActualSaveCommand = saveCommand;

            CancelCommand = cancelCommand;
            ShowListCommand = showListCommand;

            PropertiesToValidate = new List<string> { "Date", "Details" };

            InitializeCommands();
            NavigationCommands =new List<CommandBase>(){SaveCommand, CancelCommand, RefreshCommand};
            CancelCommand.Action = b => showListCommand.Execute(null);

            HenHouses = new ObservableCollection<HenHouse>(houseService.GetAll().OrderBy(h => h.Name));

            SubscribeMessages();
        }

        #region commands

        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand RefreshCommand { get; set; }

        private SaveEggProductionCommand ActualSaveCommand { get; set; }
        public ShowEggProductionListCommand ShowListCommand { get; private set; }

        public CancelCommand CancelCommand { get; private set; }
        public IList<CommandBase> NavigationCommands { get; private set; }

        private void InitializeCommands()
        {
            SaveCommand = new DelegateCommand(Save, CanSave) { Text = () => LanguageData.General_Save };
            RefreshCommand = new DelegateCommand(RefreshConsumption, CanRefresh) { Text = () => LanguageData.General_Refresh };
        }

        void Save(object param)
        {
            var production = Mapper.Map<EggProductionEntryViewModel, Models.EggProduction>(this);
            ActualSaveCommand.Production = production;
            ActualSaveCommand.Execute(production);
        }

        bool CanSave(object param)
        {
            return IsValid();
        }

        void RefreshConsumption(object param)
        {
            if (details == null || details.Count == 0) return;

            foreach (var detail in details)
            {
                detail.FeedTotal = usageService.GetDailyFeedAmount(detail.HouseId, Date);
            }
        }

        bool CanRefresh(object param)
        {
            return true;
        }

        #endregion

        #region text

        public string DateText { get { return LanguageData.EggProduction_DateField; } }

        public string HouseText { get { return LanguageData.EggProductionDetail_HouseField; } }

        public string GoodEggCountText { get { return LanguageData.EggProductionDetail_GoodEggCountField; } }

        public string RetailQuantityText { get { return LanguageData.EggProductionDetail_RetailQuantityField; } }

        public string FcrText { get { return LanguageData.EggProductionDetail_FcrField; } }

        public string CrackedEggCountText { get { return LanguageData.EggProductionDetail_CrackedEggCountField; } }

        public string NewText { get { return LanguageData.General_New; } }

        public string DeleteText { get { return LanguageData.General_Delete; } }

        #endregion

        #region properties

        private Guid id;
        private DateTime date;

        private ObservableCollection<EggProductionDetailViewModel> details;

        public Guid Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged("Id"); }
        }

        public DateTime Date
        {
            get { return date; }
            set { date = value; OnPropertyChanged("Date"); }
        }

        public DateTime DateInUTC
        {
            get { return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(Date, ConfigurationManager.AppSettings["Timezone"]); }
            set
            {
                Date = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(value, ConfigurationManager.AppSettings["Timezone"], "UTC");
            }
        }

        public ObservableCollection<EggProductionDetailViewModel> Details
        {
            get { return details; }
            set { details = value; OnPropertyChanged("Details"); }
        }

        public ObservableCollection<HenHouse> HenHouses { get; private set; } 

        #endregion

        #region validation

        public override string this[string columnName]
        {
            get
            {
                string result = null;

                switch (columnName)
                {
                    case "Details":
                        if (details == null || details.Count == 0)
                        {
                            result = LanguageData.EggProduction_RequireDetails;
                        }
                        else
                        {
                            for (int i = 0; i < details.Count; i++)
                            {
                                result = details[i].Error;
                                if (result != null)
                                    break;
                            }
                        }

                        break;
                }

                return result;
            }
        }

        

        #endregion

        #region handle messages

        void SubscribeMessages()
        {
            broker.Subscribe(CommonMessages.NewEggProductionView, OnNew);
            broker.Subscribe(CommonMessages.LoadEggProduction, OnLoad);
            broker.Subscribe(CommonMessages.SaveEggProductionSuccess, OnSaveSuccess);
            broker.Subscribe(CommonMessages.SaveEggProductionFailed, OnSaveFailed);
        }

        void UnsubscribeMessages()
        {
            broker.Unsubscribe(CommonMessages.NewEmployeeCostEntry, OnNew);
            broker.Unsubscribe(CommonMessages.LoadEmployeeCost, OnLoad);
            broker.Unsubscribe(CommonMessages.SaveEggProductionSuccess, OnSaveSuccess);
            broker.Unsubscribe(CommonMessages.SaveEggProductionFailed, OnSaveFailed);
        }

        void OnNew(object param)
        {
            Id = Guid.Empty;
            Date = DateTime.Today;
            Details = new ObservableCollection<EggProductionDetailViewModel>();

            var activeHouses = HenHouses.Where(e => e.Active).ToList();
            foreach (var house in activeHouses)
            {
                var detail = new EggProductionDetailViewModel
                {
                    HouseId = house.Id,
                    FeedTotal = usageService.GetDailyFeedAmount(house.Id, Date)
                };
                
                Details.Add(detail);
            }
        }

        void OnLoad(object param)
        {
            var loadedData = service.Get((Guid)param);

            Id = loadedData.Id;
            Date = loadedData.Date.Date;

            var loadedDatails = Mapper.Map<List<Models.EggProductionDetail>, List<EggProductionDetailViewModel>>(loadedData.Details);

            Details = new ObservableCollection<EggProductionDetailViewModel>(loadedDatails);

            RefreshConsumption(null);
        }

        void OnSaveSuccess(object param)
        {
            ShowListCommand.Execute(null);
        }

        void OnSaveFailed(object param)
        {
            MessageBox.Show(LanguageHelper.TryGetErrorMessage(param));
        }

        #endregion

        public override void Dispose()
        {
            UnsubscribeMessages();
            base.Dispose();
        }
    }
}
