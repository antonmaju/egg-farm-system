using AutoMapper;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.MasterData.Commands;
using EggFarmSystem.Client.Modules.MasterData.Views;
using EggFarmSystem.Models;
using EggFarmSystem.Resources;
using EggFarmSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace EggFarmSystem.Client.Modules.MasterData.ViewModels
{
    public class EmployeeEntryViewModel : ViewModelBase 
    {
        private readonly IEmployeeService employeeService;
        private readonly IMessageBroker messageBroker;

        public EmployeeEntryViewModel(IMessageBroker messageBroker, IEmployeeService employeeService,
                                      SaveEmployeeCommand saveCommand, CancelCommand cancelCommand)
        {
            this.employeeService = employeeService;
            this.messageBroker = messageBroker;

            PropertiesToValidate = new List<string> {"Name", "Salary"};

            ActualSaveCommand = saveCommand;
            CancelCommand = cancelCommand;
           
            InitializeCommands();
            SubscribeMessages();            
        }

        #region commands

        public CancelCommand CancelCommand { get; private set; }

        public DelegateCommand SaveCommand { get; private set; }

        public IList<CommandBase> NavigationCommands { get; private set; }

        private SaveEmployeeCommand ActualSaveCommand { get; set; }

        private void InitializeCommands()
        {
            SaveCommand = new DelegateCommand(Save, CanSave){ Text =()=> LanguageData.General_Save};

            CancelCommand.Action = broker => messageBroker.Publish(CommonMessages.ChangeMasterDataView, MasterDataTypes.Employee);

            NavigationCommands = new List<CommandBase> { SaveCommand, CancelCommand };
        }

        void Save(object param)
        {
            var employee = Mapper.Map<EmployeeEntryViewModel, Employee>(this);
            ActualSaveCommand.Execute(employee);
        }

        bool CanSave(object param)
        {
            return IsValid();
        }

        #endregion

        #region text

        public string NameText { get { return LanguageData.Employee_NameField; } }

        public string SalaryText { get { return LanguageData.Employee_SalaryField; } }

        public string ActiveText { get { return LanguageData.Employee_ActiveField; } }

        #endregion

        #region properties

        private Guid id;
        private string name;
        private long salary;
        private bool active;

        public Guid Id
        {
            get { return id; }
            set 
            { 
                id = value;
                OnPropertyChanged("Id");
            }
        }

        public string Name
        {
            get { return name; }
            set 
            { 
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public long Salary
        {
            get { return salary; }
            set { 
                salary = value;
                OnPropertyChanged("Salary");
            }
        }

        public bool Active
        {
            get { return active; }

            set
            {
                active = value;
                OnPropertyChanged("Active");
            }
        }

        #endregion

        public override string this[string columnName]
        {
            get
            {
                string result = null;

                switch (columnName)
                {
                    case "Name":
                        if (string.IsNullOrWhiteSpace(Name))
                            result = LanguageData.Employee_RequireName;
                        break;
                    case "Salary":
                        if (Salary <= 0)
                            result = LanguageData.Employee_RequireSalary;
                        break;
                }

                return result;
            }
        }

        void SubscribeMessages()
        {
            messageBroker.Subscribe(CommonMessages.NewEmployeeEntry, OnNewEmployee);
            messageBroker.Subscribe(CommonMessages.LoadEmployee, OnEditEmployee);
            messageBroker.Subscribe(CommonMessages.SaveEmployeeFailed, OnSaveEmployeeFailed);
        }

        void OnNewEmployee(object param)
        {
            Id = Guid.Empty;
            Name = string.Empty;
            Salary = 0;
        }

        void OnEditEmployee(object param)
        {
            var loadedEmployee = employeeService.Get((Guid) param);

            Id = loadedEmployee.Id;
            Name = loadedEmployee.Name;
            Salary = loadedEmployee.Salary;
            Active = loadedEmployee.Active;
        }

        void OnSaveEmployeeFailed(object param)
        {
            MessageBox.Show(LanguageHelper.TryGetErrorMessage(param));
        }

        void UnsubscribeMessages()
        {
            messageBroker.Unsubscribe(CommonMessages.NewEmployeeEntry, OnNewEmployee);
            messageBroker.Unsubscribe(CommonMessages.LoadEmployee, OnEditEmployee);
            messageBroker.Unsubscribe(CommonMessages.SaveEmployeeFailed, OnSaveEmployeeFailed);
        }

        public override void Dispose()
        {
            UnsubscribeMessages();
            base.Dispose();
        }
    }
}
