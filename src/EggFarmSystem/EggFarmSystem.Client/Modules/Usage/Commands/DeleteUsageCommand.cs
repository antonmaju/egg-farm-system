using System;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Services;

namespace EggFarmSystem.Client.Modules.Usage.Commands
{
    public class DeleteUsageCommand : CommandBase
    {
        private readonly IConsumableUsageService usageService;
        private readonly IMessageBroker messageBroker;

        public DeleteUsageCommand(IMessageBroker messageBroker, IConsumableUsageService usageService)
        {
            this.messageBroker = messageBroker;
            this.usageService = usageService;
        }

        public Guid EntityId { get; set; }

        public override void Execute(object parameter)
        {
            Guid id = Guid.Empty;

            try
            {
                id = parameter == null ? EntityId : (Guid) parameter;
                if(usageService.Delete(id))
                    messageBroker.Publish(CommonMessages.DeleteUsageSuccess, id);
                else
                    messageBroker.Publish(CommonMessages.DeleteUsageFailed, id);
            }
            catch (Exception ex)
            {
                var error = new Error { Exception = ex, Data = id };
                messageBroker.Publish(CommonMessages.DeleteUsageFailed, error);
            }
        }

        public override bool CanExecute(object parameter)
        {
            if (EntityId != Guid.Empty)
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
