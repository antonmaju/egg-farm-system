using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.MasterData.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Modules.MasterData.Commands
{
    public class EditHouseCommand :CommandBase
    {
        private readonly IMessageBroker messageBroker;
        private readonly IClientContext clientContext;

        public EditHouseCommand(IClientContext clientContext, IMessageBroker messageBroker)
        {
            Text = () => "Edit";
            this.clientContext = clientContext;
            this.messageBroker = messageBroker;
        }

        public Guid HouseId { get; set; }

        public override void Execute(object parameter)
        {
            if (clientContext.MainViewType != typeof(IMasterDataView))
                messageBroker.Publish(CommonMessages.ChangeMainView, typeof(IMasterDataView));

            Guid houseId = parameter != null ? (Guid)parameter : HouseId;

            messageBroker.Publish(CommonMessages.EditHouseView, houseId);
        }
    }
}
