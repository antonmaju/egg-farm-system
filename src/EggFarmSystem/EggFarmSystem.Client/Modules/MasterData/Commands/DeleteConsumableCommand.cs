using System.Windows;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Models;
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
            try
            {
                consumableService.Delete(entityId);
                messageBroker.Publish(CommonMessages.DeleteConsumableSuccess, entityId);
            }
            catch (Exception ex)
            {
                var error = new Error { Data = entityId, Exception = ex };
                messageBroker.Publish(CommonMessages.DeleteConsumableFailed,error);
            }
        }
    }
}
