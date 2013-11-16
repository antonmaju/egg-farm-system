using EggFarmSystem.Client.Core;
using EggFarmSystem.Models;
using EggFarmSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Modules.MasterData.Commands
{
    public  class SaveConsumableCommand : SaveMasterDataCommand<Consumable>
    {
        private readonly IMessageBroker messageBroker;
        private readonly IConsumableService consumableService;

        public SaveConsumableCommand(IMessageBroker messageBroker, IConsumableService consumableService)
            : base(messageBroker)
        {
            this.messageBroker = messageBroker;
            this.consumableService = consumableService;
        }

        protected override void OnSave(Consumable entity)
        {
            var consumable = entity as Consumable ?? Entity;

            try
            {
                consumableService.Save(consumable);
                messageBroker.Publish(CommonMessages.SaveConsumableSuccess, consumable);
            }
            catch(Exception ex)
            {
                messageBroker.Publish(CommonMessages.SaveConsumableFailed, new Error
                    {
                        Data = entity,
                        Exception = ex
                    });
            }
        }
    }
}
