using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Resources;

namespace EggFarmSystem.Client.Modules.HenDepreciation.Commands
{
    public class ShowHenDepreciationCommand : CommandBase
    {
        private readonly IMessageBroker broker;

        public ShowHenDepreciationCommand(IMessageBroker broker)
        {
            Text = () => LanguageData.HenDepreciation_Title;

            this.broker = broker;
        }

        public override void Execute(object parameter)
        {
            //broker.Publish(CommonMessages.ChangeMainView, typeof(IHenDepreciationListView));
            broker.Publish(CommonMessages.RefreshHenDepreciationList, null);
        }
    }
}
