using System.Windows;
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
    public class DeleteConsumableCommand : DeleteCommand
    {
        private readonly IConsumableService consumableService;
        private readonly IMessageBroker messageBroker;

        public DeleteConsumableCommand(IMessageBroker messageBroker, IConsumableService consumableService)
        {
            this.consumableService = consumableService;
            this.messageBroker = messageBroker;
        }

        protected override void OnDeleteData(Guid entityId)
        {
            if(consumableService.Delete(entityId))
                messageBroker.Publish(CommonMessages.DeleteConsumableSuccess, entityId);
            else
                messageBroker.Publish(CommonMessages.DeleteConsumableFailed, entityId);
        }
    }
}
