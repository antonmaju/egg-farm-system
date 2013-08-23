using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.MasterData.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Modules.MasterData.Commands
{
    public class EditHouseCommand :EditMasterDataCommand
    {
        private readonly IMessageBroker messageBroker;

        public EditHouseCommand(IClientContext clientContext, IMessageBroker messageBroker):base(messageBroker,clientContext)
        {
            this.messageBroker = messageBroker;
        }

        protected override void OnExecute(Guid id)
        {
            messageBroker.Publish(CommonMessages.EditHouseView, id);
        }
    }
}
