using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.EmployeeCost.Views;
using EggFarmSystem.Resources;

namespace EggFarmSystem.Client.Modules.EggProduction.Commands
{
    public class ShowEggProductionCommand : CommandBase
    {
        private readonly IMessageBroker broker;

        public ShowEggProductionCommand(IMessageBroker broker)
        {
            Text = () => LanguageData.EggProduction_Title;

            this.broker = broker;
        }

        public override void Execute(object parameter)
        {
            //broker.Publish(CommonMessages.ChangeMainView, typeof(IEggProductionListView));
            broker.Publish(CommonMessages.RefreshEggProductionList, null);
        }
    }
}
