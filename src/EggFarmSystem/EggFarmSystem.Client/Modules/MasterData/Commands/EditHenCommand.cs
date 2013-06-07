using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.MasterData.Views;

namespace EggFarmSystem.Client.Modules.MasterData.Commands
{
    public class EditHenCommand : CommandBase
    {
        private readonly IMessageBroker messageBroker;
        private readonly IClientContext clientContext;

        public EditHenCommand(IMessageBroker messageBroker, IClientContext clientContext)
        {
            Text = () => "Edit";
            this.messageBroker = messageBroker;
            this.clientContext = clientContext;
        }

        public Guid HenId { get; set; }

        public override void Execute(object parameter)
        {
            if (clientContext.MainViewType != typeof(IMasterDataView))
                messageBroker.Publish(CommonMessages.ChangeMainView, typeof(IMasterDataView));

            Guid henId = parameter != null ? (Guid) parameter : HenId;

            messageBroker.Publish(CommonMessages.EditHenView, henId);
        }
    }
}
