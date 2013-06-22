using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.MasterData.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Resources;

namespace EggFarmSystem.Client.Modules.MasterData.Commands
{
    public class NewEmployeeCommand : CommandBase
    {
        private readonly IMessageBroker messageBroker;
        private readonly IClientContext clientContext;
    
        public NewEmployeeCommand(IMessageBroker messageBroker, IClientContext clientContext)
        {
            Text = () => LanguageData.General_New;

            this.messageBroker = messageBroker;
            this.clientContext = clientContext;
        }

        public override void Execute(object parameter)
        {
            if (clientContext.MainViewType != typeof(IMasterDataView))
                messageBroker.Publish(CommonMessages.ChangeMainView, typeof(IMasterDataView));

            messageBroker.Publish(CommonMessages.NewEmployeeView, null);
        }
    }
}
