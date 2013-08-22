using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Resources;
using EggFarmSystem.Services;

namespace EggFarmSystem.Client.Modules.EggProduction.Commands
{
    public class DeleteEggProductionCommand : DeleteCommand
    {
        private readonly IMessageBroker broker;
        private readonly IEggProductionService service;

        public DeleteEggProductionCommand(IMessageBroker messageBroker, IEggProductionService costService)
        {
            Text = () => LanguageData.General_Delete;

            this.broker = messageBroker;
            this.service = costService;
        }

        protected override void OnDeleteData(Guid entityId)
        {
           try
           {
               service.Delete(entityId);
               broker.Publish(CommonMessages.DeleteEggProductionSuccess, entityId);
           }
           catch(Exception ex)
           {
               var error = new Error(ex, entityId); 
               broker.Publish(CommonMessages.DeleteEggProductionFailed,  error);
           }
        }
    }
}
