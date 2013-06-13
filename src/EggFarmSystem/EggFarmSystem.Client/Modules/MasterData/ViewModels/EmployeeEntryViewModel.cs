using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.MasterData.Commands;
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
        private readonly Employee employee;

        public EmployeeEntryViewModel(IMessageBroker messageBroker, IEmployeeService employeeService,
                                      SaveEmployeeCommand saveCommand)
        {
            this.employeeService = employeeService;
            this.messageBroker = messageBroker;

            employee = new Employee();
            SaveCommand = saveCommand;
            SaveCommand.Employee = employee;

            NavigationCommands = new List<CommandBase>{SaveCommand};

            SubscribeMessages();            
        }

        public IList<CommandBase> NavigationCommands { get; private set; }

        public SaveEmployeeCommand SaveCommand { get; private set; }

        #region text

        public string NameText { get { return LanguageData.Employee_NameField; } }

        public string SalaryText { get { return LanguageData.Employee_SalaryField; } }

        #endregion

        #region properties

        private Guid id;
        private string name;
        private long salary;

        public Guid Id
        {
            get { return id; }
            set 
            { 
                id = value;
                employee.Id = value;
                OnPropertyChanged("Id");
            }
        }

        public string Name
        {
            get { return name; }
            set 
            { 
                name = value;
                employee.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public long Salary
        {
            get { return salary; }
            set { 
                salary = value;
                employee.Salary = value;
                OnPropertyChanged("Salary");
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
        }

        void OnSaveEmployeeFailed(object param)
        {
            MessageBox.Show(param.ToString());
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
            SaveCommand.Employee = null;
            base.Dispose();
        }
    }
}
