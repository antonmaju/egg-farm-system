using EggFarmSystem.Client.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.MasterData.Views;

namespace EggFarmSystem.Client.Modules.MasterData.Commands
{
    public class ShowMasterDataCommand : CommandBase
    {
        private IMessageBroker broker;

        public ShowMasterDataCommand(IMessageBroker broker)
        {
            this.broker = broker;
        }

        public override void Execute(object parameter)
        {
            broker.Publish(CommonMessages.ChangeMainView, typeof(IMasterDataView));
        }
    }
}
