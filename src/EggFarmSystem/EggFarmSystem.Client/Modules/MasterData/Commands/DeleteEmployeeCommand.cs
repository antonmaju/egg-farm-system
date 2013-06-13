using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Modules.MasterData.Commands
{
    public class DeleteEmployeeCommand : CommandBase
    {
        private readonly IMessageBroker messageBroker;
        private readonly IEmployeeService employeeService;

        public DeleteEmployeeCommand(IMessageBroker messageBroker, IEmployeeService employeeService)
        {
            Text = () => "Delete";

            this.messageBroker = messageBroker;
            this.employeeService = employeeService;
        }

        public Guid EmployeeId { get; set; }

        public override void Execute(object parameter)
        {
            Guid id = parameter == null ? EmployeeId : (Guid) parameter;

            try
            {
                employeeService.Delete(id);
                messageBroker.Publish(CommonMessages.DeleteEmployeeSuccess, id);
            }
            catch(Exception ex)
            {
                messageBroker.Publish(CommonMessages.DeleteEmployeeFailed, ex.Message);
            }
        }

        public override bool CanExecute(object parameter)
        {
            if (EmployeeId != Guid.Empty)
                return true;

            try
            {
                var paramId = (Guid) parameter;
                return paramId != Guid.Empty;
            }
            catch
            {
                return false;
            }
        }
    }
}
