using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Resources;

namespace EggFarmSystem.Client.Modules.Usage.Commands
{
    public class ShowReportListCommand : CommandBase
    {
        private readonly IMessageBroker broker;

        public ShowReportListCommand(IMessageBroker broker)
        {
            this.broker = broker;

            Text = () => LanguageData.Reports_Title;
        }

        public override void Execute(object parameter)
        {
            
        }
    }
}
