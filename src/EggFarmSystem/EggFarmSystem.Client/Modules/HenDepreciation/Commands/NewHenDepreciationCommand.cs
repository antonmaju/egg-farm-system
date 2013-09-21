using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Resources;

namespace EggFarmSystem.Client.Modules.HenDepreciation.Commands
{
    public class NewHenDepreciationCommand : CommandBase
    {
        private readonly IMessageBroker broker;

        public NewHenDepreciationCommand(IMessageBroker broker)
        {
            Text = () => LanguageData.General_New;

            this.broker = broker;
        }

        public override void Execute(object parameter)
        {
            //broker.Publish(CommonMessages.ChangeMainView, typeof(IHenDepreciationEntryView));
            broker.Publish(CommonMessages.NewHenDepreciationView, null);
        }
    }
}
