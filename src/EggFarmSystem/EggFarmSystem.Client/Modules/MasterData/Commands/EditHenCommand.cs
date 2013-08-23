using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.MasterData.Views;

namespace EggFarmSystem.Client.Modules.MasterData.Commands
{
    public class EditHenCommand : EditMasterDataCommand
    {
        private readonly IMessageBroker messageBroker;

        public EditHenCommand(IMessageBroker messageBroker, IClientContext clientContext) :base(messageBroker,clientContext)
        {
            this.messageBroker = messageBroker;
        }

        protected override void OnExecute(Guid id)
        {
            messageBroker.Publish(CommonMessages.EditHenView, id);
        }

      
    }
}
