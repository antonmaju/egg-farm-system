using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.HenDepreciation.Views;
using EggFarmSystem.Resources;

namespace EggFarmSystem.Client.Modules.HenDepreciation.Commands
{
    public class ShowHenDepreciationListCommand : CommandBase
    {
        private readonly IMessageBroker broker;

        public ShowHenDepreciationListCommand(IMessageBroker broker)
        {
            Text = () => LanguageData.HenDepreciation_Title;

            this.broker = broker;
        }

        public override void Execute(object parameter)
        {
            broker.Publish(CommonMessages.ChangeMainView, typeof(IHenDepreciationListView));
            broker.Publish(CommonMessages.RefreshHenDepreciationList, null);
        }
    }
}
