using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.Usage.Views;
using EggFarmSystem.Resources;

namespace EggFarmSystem.Client.Modules.Usage.Commands
{
    public class NewUsageCommand : CommandBase
    {
        private readonly IMessageBroker messageBroker;

        public NewUsageCommand(IMessageBroker messageBroker)
        {
            Text = () => LanguageData.General_New;

            this.messageBroker = messageBroker;
        }

        public override void Execute(object parameter)
        {
            messageBroker.Publish(CommonMessages.ChangeMainView, typeof(IUsageEntryView));
            messageBroker.Publish(CommonMessages.NewUsageEntry, null);
        }

    }
}
