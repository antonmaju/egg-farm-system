using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;

namespace EggFarmSystem.Client.Modules.HenDepreciation.Commands
{
    public class NewHenDepreciationCommand : CommandBase
    {
        private readonly IMessageBroker broker;

        public NewHenDepreciationCommand(IMessageBroker broker)
        {
            this.broker = broker;
        }

        public override void Execute(object parameter)
        {
            //broker.Publish(CommonMessages.ChangeMainView, typeof(IHenDepreciationEntryView));
            broker.Publish(CommonMessages.NewHenDepreciationView, null);
        }
    }
}
