using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.MasterData.Views;
using EggFarmSystem.Models;

namespace EggFarmSystem.Client.Modules.MasterData.Commands
{
    public class NewHenCommand : CommandBase
    {
        private readonly IMessageBroker messageBroker;
        private readonly IClientContext clientContext;

        public NewHenCommand(IMessageBroker messageBroker, IClientContext clientContext)
        {
            Text = () => "New";
            this.messageBroker = messageBroker;
            this.clientContext = clientContext;
        }

        public override void Execute(object parameter)
        {
            if(clientContext.MainViewType != typeof(IMasterDataView))
                messageBroker.Publish(CommonMessages.ChangeMainView, typeof(IMasterDataView));

            messageBroker.Publish(CommonMessages.NewHenView, null);
        }
    }
}
