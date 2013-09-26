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
using EggFarmSystem.Client.Modules.EggProduction.ViewModels;
using EggFarmSystem.Client.Modules.HenDepreciation.Commands;
using EggFarmSystem.Models;
using EggFarmSystem.Resources;
using EggFarmSystem.Services;

namespace EggFarmSystem.Client.Modules.HenDepreciation.ViewModels
{
    public class HenDepreciationEntryViewModel : ViewModelBase
    {
        private readonly IMessageBroker broker;
        private readonly IHenDepreciationService service;

        public HenDepreciationEntryViewModel(IMessageBroker messageBroker, IHenDepreciationService service, IHenHouseService houseService,
            SaveHenDepreciationCommand saveCommand, CancelCommand cancelCommand, ShowHenDepreciationListCommand showListCommand)
        {
            this.broker = messageBroker;
            this.service = service;

            ActualSaveCommand = saveCommand;
            
            CancelCommand = cancelCommand;
            CancelCommand.Action = b => showListCommand.Execute(null);

            RefreshCommand = new DelegateCommand(p => OnRefresh(),p => true) {Text = () => LanguageData.General_Refresh};

            ShowListCommand = showListCommand;

            HenHouses = new ObservableCollection<HenHouse>(houseService.GetAll().OrderBy(h => h.Name));

            InitializeCommands();

            NavigationCommands = new List<CommandBase>(){SaveCommand, CancelCommand, RefreshCommand};


            SubscribeMessages();
        }

        #region commands

        public DelegateCommand SaveCommand { get; private set; }

        public CancelCommand CancelCommand { get; private set; }

        public DelegateCommand RefreshCommand { get; private set; }

        public SaveHenDepreciationCommand ActualSaveCommand { get; private set; }

        public IList<CommandBase> NavigationCommands { get; private set; }

        public ShowHenDepreciationListCommand ShowListCommand { get; private set; }

        private void InitializeCommands()
        {
            SaveCommand = new DelegateCommand(Save, CanSave) {Text = () => LanguageData.General_Save};

        }

        private void Save(object param)
        {
            var depreciation = Mapper.Map<HenDepreciationEntryViewModel, Models.HenDepreciation>(this);
            ActualSaveCommand.Depreciation = depreciation;
            ActualSaveCommand.Execute(depreciation);
        }

        private bool CanSave(object param)
        {
            return IsValid();
        }

        #endregion

        #region text

        public string DateText
        {
            get { return LanguageData.HenDepreciation_DateField; }
        }

        public string HouseText
        {
            get { return LanguageData.HenDepreciationDetail_HouseIdField; }
        }

        public string InitialPriceText
        {
            get { return LanguageData.HenDepreciationDetail_InitialPriceField; }
        }

        public string SellingPriceText
        {
            get { return LanguageData.HenDepreciationDetail_SellingPriceField; }
        }

        public string ProfitText
        {
            get { return LanguageData.HenDepreciationDetail_ProfitField; }
        }

        public string DepreciationText
        {
            get { return LanguageData.HenDepreciationDetail_DepreciationField; }
        }

        #endregion

        #region properties

        private Guid id;
        private DateTime date;

        private ObservableCollection<HenDepreciationDetailViewModel> details;

        public Guid Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged("Date");
                OnDateChanged();
            }
        }

        public DateTime DateInUTC
        {
            get { return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(Date, ConfigurationManager.AppSettings["Timezone"]); }
            set
            {
                Date = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(value, ConfigurationManager.AppSettings["Timezone"],
                                                                  "UTC");
            }
        }

        public ObservableCollection<HenDepreciationDetailViewModel> Details
        {
            get { return details; }
            set
            {
                details = value;
                OnPropertyChanged("Details");
            }
        }

        public ObservableCollection<HenHouse> HenHouses { get; private set; }

        public bool IsNew { get; private set; }

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
                            result = LanguageData.HenDepreciation_RequireDetails;
                        }
                        else
                        {
                            foreach (HenDepreciationDetailViewModel detail in details)
                            {
                                result = detail.Error;
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
            broker.Subscribe(CommonMessages.NewHenDepreciationView, OnNew);
            broker.Subscribe(CommonMessages.LoadHenDepreciation, OnLoad);
            broker.Subscribe(CommonMessages.SaveHenDepreciationSuccess, OnSaveSuccess);
            broker.Subscribe(CommonMessages.SaveHenDepreciationFailed, OnSaveFailed);
        }

        void UnsubscribeMessages()
        {
            broker.Unsubscribe(CommonMessages.NewHenDepreciationView, OnNew);
            broker.Unsubscribe(CommonMessages.LoadHenDepreciation, OnLoad);
            broker.Unsubscribe(CommonMessages.SaveHenDepreciationSuccess, OnSaveSuccess);
            broker.Unsubscribe(CommonMessages.SaveHenDepreciationFailed, OnSaveFailed);
        }

        void OnNew(object param)
        {
            IsNew = true;
            Id = Guid.Empty;
            Date = DateTime.Today;
        }

        private void OnLoad(object param)
        {
            IsNew = false;

            Models.HenDepreciation initialValues = null;

            try
            {
                initialValues = service.Get((Guid)param);
            }
            catch (Exception ex)
            {
                return;
            }

            Id = initialValues.Id;
            Date = initialValues.Date;
            var loadedDatails = Mapper.Map<List<Models.HenDepreciationDetail>, List<HenDepreciationDetailViewModel>>(initialValues.Details);
            Details = new ObservableCollection<HenDepreciationDetailViewModel>(loadedDatails);
        }

        private void OnSaveSuccess(object param)
        {
            ShowListCommand.Execute(null);
        }

        void OnSaveFailed(object param)
        {
            MessageBox.Show(LanguageHelper.TryGetErrorMessage(param));
        }

        #endregion

        void OnDateChanged()
        {
            if (!IsNew) return;

            Models.HenDepreciation initialValues = null;
            
            try
            {
                initialValues = service.GetInitialValues(Date);
            }
            catch (Exception ex)
            {
                //TODO find the best way to refactor error
                return;
            }

            var loadedDatails = Mapper.Map<List<Models.HenDepreciationDetail>, List<HenDepreciationDetailViewModel>>(initialValues.Details);
            Details= new ObservableCollection<HenDepreciationDetailViewModel>(loadedDatails);
        }
 
        void OnRefresh()
        {
            Models.HenDepreciation initialValues = service.GetInitialValues(Date);
            

            //retain current selling price
            foreach (var newDetail in initialValues.Details)
            {
                var detail = Details.FirstOrDefault(d => d.HouseId == newDetail.HouseId);

                if (detail == null) continue;

                newDetail.SellingPrice = detail.SellingPrice;
            }

            var loadedDatails = Mapper.Map<List<Models.HenDepreciationDetail>, List<HenDepreciationDetailViewModel>>(initialValues.Details);
            Details = new ObservableCollection<HenDepreciationDetailViewModel>(loadedDatails);
        }


        public override void Dispose()
        {
            UnsubscribeMessages();
            base.Dispose();
        } 
    }
}
