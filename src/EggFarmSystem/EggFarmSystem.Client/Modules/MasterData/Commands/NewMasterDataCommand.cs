using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.MasterData.Views;
using EggFarmSystem.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Modules.MasterData.Commands
{
    public abstract class NewMasterDataCommand : CommandBase
    {
        private readonly IMessageBroker messageBroker;
        private readonly IClientContext clientContext;

        protected NewMasterDataCommand(IMessageBroker messageBroker, IClientContext clientContext)
        {
            Text = () => LanguageData.General_New ;

            this.messageBroker = messageBroker;
            this.clientContext = clientContext;
        }

        public override void Execute(object parameter)
        {
            if (clientContext.MainViewType != typeof(IMasterDataView))
                messageBroker.Publish(CommonMessages.ChangeMainView, typeof(IMasterDataView));


            OnExecute(parameter);
        }

        protected abstract void OnExecute(object parameter);
    }
}
