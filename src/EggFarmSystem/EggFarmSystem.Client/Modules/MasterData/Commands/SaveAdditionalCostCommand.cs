using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Models;
using EggFarmSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Modules.MasterData.Commands
{
    public class SaveAdditionalCostCommand : SaveMasterDataCommand<AdditionalCost>
    {
        private readonly IMessageBroker broker;
        private readonly IAdditionalCostService costService;

        public SaveAdditionalCostCommand(IMessageBroker broker, IAdditionalCostService costService)
            :base(broker)
        {
            this.broker = broker;
            this.costService = costService;
        }

        protected override void OnSave(AdditionalCost entity)
        {
            try
            {
                costService.Save(entity);
                broker.Publish(CommonMessages.SaveAdditionalCostSuccess, entity);
            }
            catch (Exception ex)
            {
                broker.Publish(CommonMessages.SaveAdditionalCostFailed, new Error
                {
                    Data = entity,
                    Exception = ex
                });
            }
        }
    }
}
