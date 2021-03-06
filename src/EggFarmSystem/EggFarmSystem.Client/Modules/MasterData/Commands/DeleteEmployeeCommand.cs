﻿using System.Windows;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Resources;
using EggFarmSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Modules.MasterData.Commands
{
    public class DeleteEmployeeCommand : DeleteCommand
    {
        private readonly IMessageBroker messageBroker;
        private readonly IEmployeeService employeeService;

        public DeleteEmployeeCommand(IMessageBroker messageBroker, IEmployeeService employeeService)
        {
            this.messageBroker = messageBroker;
            this.employeeService = employeeService;
        }

        protected override void OnDeleteData(Guid entityId)
        {
            try
            {
                employeeService.Delete(entityId);
                messageBroker.Publish(CommonMessages.DeleteEmployeeSuccess, entityId);
            }
            catch(Exception ex)
            {
                var error = new Error(ex, entityId);
                messageBroker.Publish(CommonMessages.DeleteEmployeeFailed, error);
            }    
        }

       
    }
}
