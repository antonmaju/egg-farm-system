using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.EmployeeCost.Views;
using EggFarmSystem.Resources;

namespace EggFarmSystem.Client.Modules.EmployeeCost.Commands
{
    public class ShowEmployeeCostCommand : CommandBase
    {
        private readonly IMessageBroker broker;

        public ShowEmployeeCostCommand(IMessageBroker broker)
        {
            Text = () => LanguageData.EmployeeCost_Title;

            this.broker = broker;
        }

        public override void Execute(object parameter)
        {
            broker.Publish(CommonMessages.ChangeMainView, typeof(IEmployeeCostListView));
            broker.Publish(CommonMessages.RefreshEmployeeCostList, null);
        }
    }
}
