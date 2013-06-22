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
    public class NewHouseCommand :CommandBase
    {
        private IMessageBroker messageBroker;
        private IClientContext clientContext;

        public NewHouseCommand(IClientContext clientContext, IMessageBroker messageBroker)
        {
            Text = () => LanguageData.General_New;

            this.messageBroker = messageBroker;
            this.clientContext = clientContext;
        }

        public override void Execute(object parameter)
        {
            if (clientContext.MainViewType != typeof(IMasterDataView))
                messageBroker.Publish(CommonMessages.ChangeMainView, typeof(IMasterDataView));

            messageBroker.Publish(CommonMessages.NewHouseView, null);
        }
    }
}
