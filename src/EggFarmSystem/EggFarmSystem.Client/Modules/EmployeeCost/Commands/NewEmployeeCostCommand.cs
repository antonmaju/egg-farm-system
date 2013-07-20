using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Resources;

namespace EggFarmSystem.Client.Modules.EmployeeCost.Commands
{
    public class NewEmployeeCostCommand : CommandBase
    {
        private readonly IMessageBroker broker;

        public NewEmployeeCostCommand(IMessageBroker broker)
        {
            Text = () => LanguageData.General_New;
            this.broker = broker;
        }

        public override void Execute(object parameter)
        {
            //broker.Publish(CommonMessages.ChangeMainView, typeof(IEmployeeCostEntryView));
            broker.Publish(CommonMessages.NewEmployeeCostEntry, null);
        }
    }
}
