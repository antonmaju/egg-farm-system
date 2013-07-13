using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Resources;

namespace EggFarmSystem.Client.Commands
{
    public class RefreshCommand : CommandBase
    {
        private readonly IMessageBroker messageBroker;

        public RefreshCommand(IMessageBroker broker)
        {
            Text = () => LanguageData.General_Refresh;
            this.messageBroker = broker;
        }

        public string MessageName { get; set; }

        public object Parameter { get; set; }

        public override void Execute(object parameter)
        {
            messageBroker.Publish(MessageName, Parameter);
        }

        public override bool CanExecute(object parameter)
        {
            return ! string.IsNullOrWhiteSpace(MessageName);
        }
    }
}
