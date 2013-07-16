using System.Windows;
using EggFarmSystem.Client.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Resources;
using EggFarmSystem.Services;

namespace EggFarmSystem.Client.Modules.MasterData.Commands
{
    public class DeleteHouseCommand : DeleteCommand
    {
        private readonly IHenHouseService houseService;
        private readonly IMessageBroker messageBroker;

        public DeleteHouseCommand(IMessageBroker messageBroker, IHenHouseService houseService)
  
        {
            this.houseService = houseService;
            this.messageBroker = messageBroker;
        }

        protected override void OnDeleteData(Guid entityId)
        {
            try
            {
                houseService.Delete(entityId);
                messageBroker.Publish(CommonMessages.DeleteHouseSuccess, entityId);
            }
            catch (Exception ex)
            {
                var error = new Error(ex, entityId);
                messageBroker.Publish(CommonMessages.DeleteHouseFailed, error);
            }
        }
    }
}
