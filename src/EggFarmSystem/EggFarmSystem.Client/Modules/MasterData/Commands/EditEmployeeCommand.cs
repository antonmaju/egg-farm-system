using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.MasterData.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Modules.MasterData.Commands
{
    public class EditEmployeeCommand : EditMasterDataCommand
    {
        private readonly IMessageBroker messageBroker;

        public EditEmployeeCommand(IMessageBroker messageBroker, IClientContext clientContext):base(messageBroker, clientContext)
        {
            this.messageBroker = messageBroker;
        }

        protected override void OnExecute(Guid id)
        {
            messageBroker.Publish(CommonMessages.EditEmployeeView, id);
        }
    }
}
