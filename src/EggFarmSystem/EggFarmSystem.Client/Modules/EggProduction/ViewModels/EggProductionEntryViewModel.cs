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

        public EggProductionEntryViewModel(IMessageBroker broker, IEggProductionService service,
            IHenHouseService houseService, SaveEggProductionCommand saveCommand, CancelCommand cancelCommand,
            ShowEggProductionListCommand showListCommand)
        {
            this.broker = broker;
            this.service = service;
            ActualSaveCommand = saveCommand;

            CancelCommand = cancelCommand;
            ShowListCommand = showListCommand;

            InitializeCommands();
            NavigationCommands =new List<CommandBase>(){SaveCommand, CancelCommand};
            CancelCommand.Action = b => showListCommand.Execute(null);

            HenHouses = new ObservableCollection<HenHouse>(houseService.GetAll());

            SubscribeMessages();
        }

        #region commands

        public DelegateCommand AddDetailCommand { get; private set; }
        public DelegateCommand<int> DeleteDetailCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }

        private SaveEggProductionCommand ActualSaveCommand { get; set; }
        public ShowEggProductionListCommand ShowListCommand { get; private set; }

        public CancelCommand CancelCommand { get; private set; }
        public IList<CommandBase> NavigationCommands { get; private set; }

        

        private void InitializeCommands()
        {
            SaveCommand = new DelegateCommand(Save, CanSave) { Text = () => LanguageData.General_Save };

            AddDetailCommand = new DelegateCommand(AddDetail, CanAddDetail) { Text = () => LanguageData.General_New };

            DeleteDetailCommand = new DelegateCommand<int>(DeleteDetail, CanDeleteDetail) { Text = () => LanguageData.General_Delete };
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

        void AddDetail(object param)
        {
            
        }

        bool CanAddDetail(object param)
        {
            return true;
        }

        void DeleteDetail(int param)
        {
            int index = Convert.ToInt32(DeleteDetailCommand.Tag);
            details.RemoveAt(index);
            DeleteDetailCommand.Tag = -1;
        }

        bool CanDeleteDetail(int param)
        {
            return DeleteDetailCommand.Tag > -1;
        }

        #endregion

        #region text

        public string DateText { get { return LanguageData.EggProduction_DateField; } }

        public string HouseText { get { return LanguageData.EggProductionDetail_HouseField; } }

        public string GoodEggCountText { get { return LanguageData.EggProductionDetail_InvalidGoodEggCount; } }

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
            set { id = value; }
        }

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
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

        private static readonly string[] PropertiesToValidate =
            {
                "Date",
                "Details"
            };

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

        private bool IsValid()
        {
            bool valid = true;

            foreach (var prop in PropertiesToValidate)
            {
                if (this[prop] != null)
                {
                    valid = false;
                    break;
                }
            }

            return valid;
        }

        #endregion

        #region handle messages

        void SubscribeMessages()
        {
            broker.Subscribe(CommonMessages.NewEggProductionView, OnNew);
            broker.Subscribe(CommonMessages.LoadEggProduction, OnLoad);
            broker.Subscribe(CommonMessages.SaveEmployeeCostSuccess, OnSaveSuccess);
            broker.Subscribe(CommonMessages.SaveEmployeeCostFailed, OnSaveFailed);
        }

        void UnsubscribeMessages()
        {
            broker.Unsubscribe(CommonMessages.NewEmployeeCostEntry, OnNew);
            broker.Unsubscribe(CommonMessages.LoadEmployeeCost, OnLoad);
            broker.Unsubscribe(CommonMessages.SaveEmployeeCostSuccess, OnSaveSuccess);
            broker.Unsubscribe(CommonMessages.SaveEmployeeCostFailed, OnSaveFailed);
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

        EggProductionDetailViewModel CreateNewDetail()
        {
            return new EggProductionDetailViewModel();
        }

        public override void Dispose()
        {
            UnsubscribeMessages();
            base.Dispose();
        }
    }
}
