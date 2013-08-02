using EggFarmSystem.Client.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Modules.MasterData.Commands
{
    public class EditAdditionalCostCommand : EditMasterDataCommand
    {
        private readonly IMessageBroker broker;

        public EditAdditionalCostCommand(IMessageBroker broker, IClientContext clientContext)
            : base(broker, clientContext)
        {
            this.broker = broker;
        }

        protected override void OnExecute(Guid id)
        {
            broker.Publish(CommonMessages.EditAdditionalCostView, id);
        }
    }
}
