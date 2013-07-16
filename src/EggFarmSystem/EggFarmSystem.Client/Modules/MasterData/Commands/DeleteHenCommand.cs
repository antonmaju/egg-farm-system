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
    public class DeleteHenCommand : DeleteCommand
    {
        private readonly IHenService henService;
        private readonly IMessageBroker messageBroker;

        public DeleteHenCommand(IMessageBroker messageBroker, IHenService henService)
        {
            this.henService = henService;
            this.messageBroker = messageBroker;
        }

        protected override void OnDeleteData(Guid entityId)
        {
            try
            {
                henService.Delete(entityId);
                messageBroker.Publish(CommonMessages.DeleteHenSuccess, entityId);
            }
            catch(Exception ex)
            {
                var error = new Error(ex, entityId);
                messageBroker.Publish(CommonMessages.DeleteHenFailed, error);
            }              
        }
    }
}
