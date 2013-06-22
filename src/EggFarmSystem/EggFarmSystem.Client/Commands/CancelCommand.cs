using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Resources;

namespace EggFarmSystem.Client.Commands
{
    public class CancelCommand : CommandBase
    {
        private readonly IMessageBroker messageBroker;
        
        public CancelCommand(IMessageBroker messageBroker)
        {
            Text = () => LanguageData.General_Cancel;

            this.messageBroker = messageBroker;
        }

        public Action<IMessageBroker> Action { get; set; }

        public override void Execute(object parameter)
        {
            if (Action != null)
                Action(messageBroker);
        }

        public override bool CanExecute(object parameter)
        {
            return Action != null;
        }
    }
}
