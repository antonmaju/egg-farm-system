using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using AutoMapper;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.EmployeeCost.Commands;
using EggFarmSystem.Resources;
using EggFarmSystem.Services;

namespace EggFarmSystem.Client.Modules.EmployeeCost.ViewModels
{
    public class EmployeeCostEntryViewModel : ViewModelBase
    {
        private readonly IMessageBroker messageBroker;
        private readonly IEmployeeCostService costService;
        private readonly IEmployeeService employeeService; 

        public EmployeeCostEntryViewModel(
            IMessageBroker messageBroker, 
            IEmployeeCostService costService,
            IEmployeeService employeeService,
            SaveEmployeeCostCommand saveCostCommand,
            CancelCommand cancelCommand, ShowEmployeeCostCommand showListCommand
            )
        {
            this.messageBroker = messageBroker;
            this.costService = costService;
            this.employeeService = employeeService;
            ActualSaveCommand = saveCostCommand;

            CancelCommand = cancelCommand;
            InitializeCommands();
            NavigationCommands = new List<CommandBase>{CancelCommand};
            CancelCommand.Action = broker => showListCommand.Execute(null);
        }

        #region commands

        public DelegateCommand AddDetailCommand { get; private set; }
        public DelegateCommand<int> DeleteDetailCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }
        private SaveEmployeeCostCommand ActualSaveCommand { get; set; }
        public ShowEmployeeCostCommand ShowCostListCommand { get; private set; }
        
        public CancelCommand CancelCommand { get; private set; }
        public IList<CommandBase> NavigationCommands { get; private set; }

        private void InitializeCommands()
        {
            SaveCommand = new DelegateCommand(Save, CanSave) {Text = () => LanguageData.General_Save};

            AddDetailCommand = new DelegateCommand(AddDetail, CanAddDetail) {Text =() => LanguageData.General_New};

            DeleteDetailCommand = new DelegateCommand<int>(DeleteDetail, CanDeleteDetail) { Text = () => LanguageData.General_Delete };
        }

        void Save(object param)
        {
            var cost = Mapper.Map<EmployeeCostEntryViewModel, Models.EmployeeCost>(this);
            ActualSaveCommand.Cost = cost;
            ActualSaveCommand.Execute(cost);
        }

        bool CanSave(object param)
        {
            var isValid = IsValid();
            return isValid;
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
           
        }

        bool CanDeleteDetail(int param)
        {
            return true;
        }

        #endregion

        #region text

        public string DateText { get { return LanguageData.EmployeeCost_DateField; } }

        public string TotalText { get { return LanguageData.EmployeeCost_TotalField; } }

        public string PresentText { get { return LanguageData.EmployeeCostDetail_PresentField; } }

        public string SalaryText { get { return LanguageData.EmployeeCostDetail_SalaryField; } }

        public string EmployeeText { get { return LanguageData.EmployeeCostDetail_EmployeeField; } }

        public string DescriptionText { get { return LanguageData.EmployeeCostDetail_DescriptionField; } }

        public string NewText { get { return LanguageData.General_New; } }

        public string DeleteText { get { return LanguageData.General_Delete; } }

        #endregion

        #region properties

        private Guid id;
        private DateTime date;
        private long total;
        private ObservableCollection<EmployeeCostDetailViewModel> details;

        public Guid Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged("Id");}
        }

        public DateTime Date
        {
            get { return date; }
            set { date = value; OnPropertyChanged("Date"); }
        }

        public long Total
        {
            get { return total; }
            set { total = value; OnPropertyChanged("Total"); }
        }

        public ObservableCollection<EmployeeCostDetailViewModel> Details
        {
            get { return details; }
            set { details = value; OnPropertyChanged("Details"); }
        }

        #endregion

        #region validation

        private static readonly string[] PropertiesToValidate =
            {
                "Date",
                "Total",
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
                            result = LanguageData.Usage_RequireDetails;
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

        void SubscribeMessages()
        {
            messageBroker.Subscribe(CommonMessages.NewEmployeeCostEntry, OnNewCost);
            messageBroker.Subscribe(CommonMessages.LoadEmployeeCost, OnLoadCost);
            messageBroker.Subscribe(CommonMessages.SaveEmployeeCostSuccess, OnSaveCostSuccess);
            messageBroker.Subscribe(CommonMessages.SaveEmployeeCostFailed, OnSaveCostFailed);
        }

        void UnsubscribeMessages()
        {
            messageBroker.Unsubscribe(CommonMessages.NewEmployeeCostEntry, OnNewCost);
            messageBroker.Unsubscribe(CommonMessages.LoadEmployeeCost, OnLoadCost);
            messageBroker.Unsubscribe(CommonMessages.SaveEmployeeCostSuccess, OnSaveCostSuccess);
            messageBroker.Unsubscribe(CommonMessages.SaveEmployeeCostFailed, OnSaveCostFailed);
        }

        void OnNewCost(object param)
        {
            Id = Guid.Empty;
            Total = 0;
            Date = DateTime.Today;
            Details =new ObservableCollection<EmployeeCostDetailViewModel>();
            Details.Add(new EmployeeCostDetailViewModel());
        }

        void OnLoadCost(object param)
        {
            var loadedCost = costService.Get((Guid) param);

            Id = loadedCost.Id;
            Total = loadedCost.Total;
            Date = loadedCost.Date;

            var loadedDatails = Mapper.Map<List<Models.EmployeeCostDetail>, List<EmployeeCostDetailViewModel>>(loadedCost.Details);

            if (loadedDatails != null)
            {
                foreach (var loadedDetail in loadedDatails)
                {
                    loadedDetail.PropertyChanged += loadedDetail_PropertyChanged;
                }
            } 
        }

        void loadedDetail_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            
        }

        void OnSaveCostSuccess(object param)
        {
            this.ShowCostListCommand.Execute(null);
        }

        void OnSaveCostFailed(object param)
        {
            
        }

        EmployeeCostDetailViewModel CreateNewDetail()
        {
            var detail = new EmployeeCostDetailViewModel();
            detail.PropertyChanged += detail_PropertyChanged;
            return detail;
        }

        private void CalculateTotal()
        {
            Total = details.Sum(d => d.Salary);
        }

        private void SetDefaultUnitPrice(EmployeeCostDetailViewModel detail)
        {
            if (detail == null) return;

            var employee =employeeService.Get(detail.EmployeeId);
            detail.Salary = employee.Salary;
        }

        void detail_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Salary")
            {
               CalculateTotal();
            }
            else if (e.PropertyName == "ConsumableId")
            {
                SetDefaultUnitPrice(sender as EmployeeCostDetailViewModel);
            }
        }

        public override void Dispose()
        {
            UnsubscribeMessages();
            base.Dispose();
        }
    }
}
