using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.Dashboard.Views;

namespace EggFarmSystem.Client.Modules.Dashboard.Commands
{
    public class ShowDashboardCommand : CommandBase
    {
        private readonly IMessageBroker broker;

        public ShowDashboardCommand(IMessageBroker broker)
        {
            this.broker = broker;
        }

        public override void Execute(object parameter)
        {
            broker.Publish(CommonMessages.ChangeMainView, typeof(IDashboardView));
        }
    }
}
