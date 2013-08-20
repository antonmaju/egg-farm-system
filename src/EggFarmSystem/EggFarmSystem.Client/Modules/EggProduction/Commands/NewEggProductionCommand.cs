using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;

using EggFarmSystem.Resources;

namespace EggFarmSystem.Client.Modules.EggProduction.Commands
{
    public class NewEggProductionCommand : CommandBase
    {
        private readonly IMessageBroker broker;
    
        public NewEggProductionCommand(IMessageBroker broker)
        {
            Text = () => LanguageData.General_New;

            this.broker = broker;
        }

        public override void Execute(object parameter)
        {
            //broker.Publish(CommonMessages.ChangeMainView, typeof(IEggProductionEntryView));
            broker.Publish(CommonMessages.NewEggProductionView, null);
        } 
    }
}
