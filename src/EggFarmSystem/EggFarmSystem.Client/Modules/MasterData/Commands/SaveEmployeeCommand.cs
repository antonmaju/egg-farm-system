using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Models;
using EggFarmSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Modules.MasterData.Commands
{
    public class SaveEmployeeCommand : CommandBase
    {
        private IEmployeeService employeeService;
        private IMessageBroker messageBroker;

        public SaveEmployeeCommand(IMessageBroker messageBroker, IEmployeeService employeeService)
        {
            Text = () => "Save";
            this.employeeService = employeeService;
            this.messageBroker = messageBroker;
        }

        public Employee Employee { get; set; }

        public override void Execute(object parameter)
        {
            var employee = parameter as Employee ?? Employee;

            if (employee == null)
                return;

            try
            {
                employeeService.Save(employee);
                messageBroker.Publish(CommonMessages.SaveEmployeeSuccess, employee);
            }
            catch (Exception ex)
            {
                var error = new Error(ex, employee);
                messageBroker.Publish(CommonMessages.SaveEmployeeFailed, error);
            }
        }
    }
}
