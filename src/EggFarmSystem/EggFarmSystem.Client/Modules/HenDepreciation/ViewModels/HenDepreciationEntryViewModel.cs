using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.HenDepreciation.Commands;
using EggFarmSystem.Resources;
using EggFarmSystem.Services;

namespace EggFarmSystem.Client.Modules.HenDepreciation.ViewModels
{
    public class HenDepreciationEntryViewModel : ViewModelBase
    {
        private readonly IMessageBroker messageBroker;
        private readonly IHenDepreciationService service;

        public HenDepreciationEntryViewModel(IMessageBroker messageBroker, IHenDepreciationService service)
        {
            this.messageBroker = messageBroker;
            this.service = service;
        }

        #region commands

        public DelegateCommand SaveCommand { get; private set; }

        public CancelCommand CancelCommand { get; private set; }

        public SaveHenDepreciationCommand ActualSaveCommand { get; private set; }

        public IList<CommandBase> NavigationCommands { get; private set; }

        private void InitializeCommands()
        {
            SaveCommand = new DelegateCommand(Save, CanSave) {Text = () => LanguageData.General_Save};

        }

        private void Save(object param)
        {

        }

        private bool CanSave(object param)
        {
            return false;
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

        private ObservableCollection<HenDepreciationEntryViewModel> details;

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

        public ObservableCollection<HenDepreciationEntryViewModel> Details
        {
            get { return details; }
            set
            {
                details = value;
                OnPropertyChanged("Details");
            }
        }

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
                            for (int i = 0; i <details.Count; i++)
                            {
                                result = details[i].Error;
                                if (result != null)
                                    break;
                            }
                        }
                        break;
                }

                return base[columnName];
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
            
        }

        void UnsubscribeMessages()
        {
            
        }

        void OnNew(object param)
        {
            
        }

        private void OnLoad(object param)
        {

        }

        private void OnSaveSuccess(object param)
        {

        }

        void OnSaveFailed(object param)
        {
            
        }

        #endregion

        public override void Dispose()
        {
            base.Dispose();
        } 
    }
}
