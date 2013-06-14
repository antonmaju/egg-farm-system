using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Modules.MasterData.Commands
{
    public class NewConsumableCommand : NewMasterDataCommand
    {
        private readonly IMessageBroker messageBroker;

        public NewConsumableCommand(IMessageBroker messageBroker, IClientContext clientContext)
            : base(messageBroker, clientContext)
        {
            this.messageBroker = messageBroker;
        }

        protected override void OnExecute(object parameter)
        {
            messageBroker.Publish(CommonMessages.NewConsumableView, null);
        }
    }
}
