using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AutoMapper;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.Usage.Commands;
using EggFarmSystem.Client.Modules.Usage.Views;
using EggFarmSystem.Models;
using EggFarmSystem.Resources;
using EggFarmSystem.Services;

namespace EggFarmSystem.Client.Modules.Usage.ViewModels
{
    public class UsageEntryViewModel : ViewModelBase
    {
        private readonly IMessageBroker messageBroker;
        private readonly IConsumableUsageService usageService;
        private readonly IConsumableService consumableService;
        private readonly IHenHouseService houseService;

        private DelegateCommand saveCommand, addDetailCommand;
        private DelegateCommand<int> deleteDetailCommand;
        private SaveUsageCommand saveUsageCommand;
        private ShowUsageCommand showListCommand;

        public UsageEntryViewModel(IMessageBroker messageBroker, IConsumableUsageService usageService,
            IHenHouseService houseService, IConsumableService consumableService,
            SaveUsageCommand saveUsageCommand, CancelCommand cancelCommand, ShowUsageCommand showListCommand
            )
        {
            this.messageBroker = messageBroker;
            this.usageService = usageService;
            this.houseService = houseService;
            this.consumableService = consumableService;

            this.saveUsageCommand = saveUsageCommand;
            this.showListCommand = showListCommand;

            CancelCommand = cancelCommand;

            InitializeDelegateCommands();

            NavigationCommands = new List<CommandBase>(){saveCommand, CancelCommand};

            HouseList = new ObservableCollection<HenHouse>(houseService.GetAll());
            ConsumableList = new ObservableCollection<Consumable>(consumableService.GetAll());

            SubscribeMessages();
        }

        private void InitializeDelegateCommands()
        {
            saveCommand = new DelegateCommand(Save, CanSave);
            saveCommand.Text = () => LanguageData.General_Save;

            addDetailCommand = new DelegateCommand(AddDetail, CanAddDetail);
            addDetailCommand.Text = () => LanguageData.General_New;

            deleteDetailCommand = new DelegateCommand<int>(DeleteDetail, CanDeleteDetail);
            deleteDetailCommand.Text = () => LanguageData.General_Delete;

            CancelCommand.Action = broker => showListCommand.Execute(null);
        }

        public DelegateCommand AddDetailCommand { get { return addDetailCommand; } }

        public DelegateCommand<int> DeleteDetailCommand { get { return deleteDetailCommand; } }

        public CancelCommand CancelCommand { get; private set; }

        public IList<CommandBase> NavigationCommands { get; private set; }

        public ObservableCollection<HenHouse> HouseList { get; private set; }

        public ObservableCollection<Consumable> ConsumableList { get; private set; } 

        #region text

        public string DateText { get { return LanguageData.Usage_DateField; } }

        public string TotalText { get { return LanguageData.Usage_TotalField; } }

        public string HouseText { get { return LanguageData.Usage_HouseField; } }

        public string ConsumableText { get { return LanguageData.Usage_ConsumableField; } }

        public string CountText { get { return LanguageData.Usage_CountField; } }

        public string UnitPriceText { get { return LanguageData.Usage_UnitPriceField; } }

        public string SubTotalText { get { return LanguageData.Usage_SubTotalField; } }

        public string NewText { get { return LanguageData.General_New; } }

        public string DeleteText { get { return LanguageData.General_Delete; } }

        #endregion

        #region properties

        private Guid id;
        private DateTime date;
        private long total;
        
        private ObservableCollection<UsageDetailViewModel> details;

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

        public long Total
        {
            get { return total; }
            set 
            { 
                total = value;
                OnPropertyChanged("Total");
            }
        }

        public ObservableCollection<UsageDetailViewModel> Details
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

        public override string this[string columnName]
        {
            get
            {
                string result = null;

                switch (columnName)
                {
                    case "Details":
                        if (details == null || details.Count == 0)
                            result = LanguageData.Usage_RequireDetails;
                        else
                        {
                            for (int i = 0; i < details.Count; i++)
                            {
                                result = details[i].Error;
                                if(result != null)
                                    break;
                            }
                        }

                        break;
                }

                return result;
            }
        }

        private static readonly string[] PropertiesToValidate =
            {
                "Date",
                "Total",
                "Details"
            };
        

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


        void SubscribeMessages()
        {
            messageBroker.Subscribe(CommonMessages.NewUsageEntry, OnNewUsage);
            messageBroker.Subscribe(CommonMessages.LoadUsage, OnLoadUsage);
            messageBroker.Subscribe(CommonMessages.SaveUsageSuccess, OnSaveUsageSuccess);
            messageBroker.Subscribe(CommonMessages.SaveUsageFailed, OnSaveUsageFailed);
        }

        void OnNewUsage(object param)
        {
            Id = Guid.Empty;
            Total = 0;
            Date = DateTime.Today;
            Details = new ObservableCollection<UsageDetailViewModel>();
            
            Details.Add(CreateNewDetail());
        }

        void OnLoadUsage(object param)
        {
            var loadedUsage = usageService.Get((Guid) param);

            Id = loadedUsage.Id;
            Total = loadedUsage.Total;
            Date = loadedUsage.Date;
            var loadedDetails = Mapper.Map<List<ConsumableUsageDetail>, List<UsageDetailViewModel>>(loadedUsage.Details);
            if (loadedDetails != null)
            {
                foreach (var loadedDetail in loadedDetails)
                {
                    loadedDetail.PropertyChanged += detail_PropertyChanged;
                }
            }

            Details = new ObservableCollection<UsageDetailViewModel>(loadedDetails);    
        }

        void OnSaveUsageFailed(object param)
        {
            MessageBox.Show(LanguageHelper.TryGetErrorMessage(param));
        }

        void UnsubscribeMessages()
        {
            messageBroker.Unsubscribe(CommonMessages.NewUsageEntry, OnNewUsage);
            messageBroker.Unsubscribe(CommonMessages.LoadUsage, OnLoadUsage);
            messageBroker.Subscribe(CommonMessages.SaveUsageSuccess, OnSaveUsageSuccess);
            messageBroker.Unsubscribe(CommonMessages.SaveUsageFailed, OnSaveUsageFailed);
        }

        void OnSaveUsageSuccess(object param)
        {
            this.showListCommand.Execute(null);
        }
        
        void Save(object param)
        {
            var usage = Mapper.Map<UsageEntryViewModel, ConsumableUsage>(this);
            saveUsageCommand.Usage = usage;
            saveUsageCommand.Execute(usage);
        }

        bool CanSave(object param)
        {
            var isValid = IsValid();
            return isValid;
        }

        void AddDetail(object param)
        {
            details.Add(CreateNewDetail());
        }

        bool CanAddDetail(object param)
        {
            return true;
        }

        void DeleteDetail(int param)
        {
            int index = Convert.ToInt32(deleteDetailCommand.Tag);
            var detail = details[index];
            detail.PropertyChanged -=detail_PropertyChanged;
            details.RemoveAt(index);
            deleteDetailCommand.Tag = -1;
        }

        bool CanDeleteDetail(int param)
        {
            return deleteDetailCommand.Tag > -1;
        }


        UsageDetailViewModel CreateNewDetail()
        {
            var detail = new UsageDetailViewModel();
            detail.PropertyChanged += detail_PropertyChanged;
            return detail;
        }

        void detail_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SubTotal")
            {
                CalculateTotal();
            }
            else if (e.PropertyName == "ConsumableId")
            {
                SetDefaultUnitPrice(sender as UsageDetailViewModel);
            }
        }

        private void CalculateTotal()
        {
            Total = details.Sum(d => d.SubTotal);
        }

        private void SetDefaultUnitPrice(UsageDetailViewModel detail)
        {
            if (detail == null) return;

            var consumable = consumableService.Get(detail.ConsumableId);
            detail.UnitPrice = consumable.UnitPrice;
        }

        public override void Dispose()
        {
            UnsubscribeMessages();
            base.Dispose();
        }
    }
}
