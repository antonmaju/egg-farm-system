using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.MasterData.Views;
using EggFarmSystem.Client.Modules.Usage.Views;
using EggFarmSystem.Resources;

namespace EggFarmSystem.Client.Modules.Usage.Commands
{
    public class ShowUsageCommand : CommandBase
    {
        private readonly IMessageBroker broker;

        public ShowUsageCommand(IMessageBroker broker)
        {
            Text = () => LanguageData.Usage_Title;

            this.broker = broker;
        }

        public override void Execute(object parameter)
        {
            broker.Publish(CommonMessages.ChangeMainView, typeof(IUsageListView));
        }
    }
}
