using EggFarmSystem.Client.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Modules.MasterData.Commands
{
    public class NewAdditionalCostCommand : NewMasterDataCommand
    {
        private readonly IMessageBroker broker;

        public NewAdditionalCostCommand(IMessageBroker broker, IClientContext clientContext)
            : base(broker, clientContext)
        {
            this.broker = broker;
        }

        protected override void OnExecute(object parameter)
        {
            broker.Publish(CommonMessages.NewAdditionalCostView, null);
        }
    }
}
