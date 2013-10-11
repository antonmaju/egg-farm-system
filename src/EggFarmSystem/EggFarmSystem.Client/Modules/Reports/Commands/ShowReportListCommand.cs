using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.Reports.Views;

namespace EggFarmSystem.Client.Modules.Reports.Commands
{
    public class ShowReportListCommand : CommandBase
    {
        private readonly IMessageBroker broker;

        public ShowReportListCommand(IMessageBroker broker)
        {
            this.broker = broker;
        }

        public override void Execute(object parameter)
        {
            broker.Publish(CommonMessages.ChangeMainView, typeof(IReportListView));
        }
    }
}
