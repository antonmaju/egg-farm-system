using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Modules.MasterData.Commands
{
    public class DeleteAdditionalCostCommand : DeleteCommand
    {
        private readonly IAdditionalCostService costService;
        private readonly IMessageBroker messageBroker;

        public DeleteAdditionalCostCommand(IAdditionalCostService costService, IMessageBroker messageBroker)
        {
            this.costService = costService;
            this.messageBroker = messageBroker;
        }

        protected override void OnDeleteData(Guid entityId)
        {
            try
            {
                costService.Delete(entityId);
                messageBroker.Publish(CommonMessages.DeleteAdditionalCostSuccess, entityId);
            }
            catch (Exception ex)
            {
                var error = new Error { Data = entityId, Exception = ex };
                messageBroker.Publish(CommonMessages.DeleteAdditionalCostFailed, error);
            }
        }
    }
}
