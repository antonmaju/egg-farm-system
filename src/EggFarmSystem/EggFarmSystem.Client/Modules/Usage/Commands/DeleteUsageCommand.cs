using System;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Models;
using EggFarmSystem.Services;

namespace EggFarmSystem.Client.Modules.Usage.Commands
{
    public class DeleteUsageCommand : DeleteCommand
    {
        private readonly IMessageBroker broker;
        private readonly IConsumableUsageService usageService;

        public DeleteUsageCommand(IMessageBroker broker, IConsumableUsageService usageService)
        {
            this.broker = broker;
            this.usageService = usageService;
        }

        protected override void OnDeleteData(Guid entityId)
        {
            try
            {
                this.usageService.Delete(entityId);
                broker.Publish(CommonMessages.DeleteUsageSuccess, entityId);
            }
            catch(ServiceException ex)
            {
                broker.Publish(CommonMessages.DeleteUsageFailed, entityId);
            }   
        }
    }
}
