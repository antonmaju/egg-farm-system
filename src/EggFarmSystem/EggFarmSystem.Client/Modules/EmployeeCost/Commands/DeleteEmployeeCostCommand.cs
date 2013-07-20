using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Models;
using EggFarmSystem.Resources;
using EggFarmSystem.Services;

namespace EggFarmSystem.Client.Modules.EmployeeCost.Commands
{
    public class DeleteEmployeeCostCommand : DeleteCommand
    {
        private readonly IMessageBroker broker;
        private readonly IEmployeeCostService costService;

        public DeleteEmployeeCostCommand(IMessageBroker messageBroker, IEmployeeCostService costService)
        {
            Text = () => LanguageData.General_Delete;

            this.broker = messageBroker;
            this.costService = costService;
        }

        protected override void OnDeleteData(Guid entityId)
        {
           try
           {
               costService.Delete(entityId);
               broker.Publish(CommonMessages.DeleteEmployeeCostSuccess, entityId);
           }
           catch(Exception ex)
           {
               var error = new Error(ex, entityId); 
               broker.Publish(CommonMessages.DeleteEmployeeCostFailed,  error);
           }

        }
    }
}
