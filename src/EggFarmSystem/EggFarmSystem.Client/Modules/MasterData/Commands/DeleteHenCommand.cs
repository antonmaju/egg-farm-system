using System.Windows;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Resources;
using EggFarmSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Modules.MasterData.Commands
{
    public class DeleteHenCommand : DeleteMasterDataCommand
    {
        private readonly IHenService henService;
        private readonly IMessageBroker messageBroker;

        public DeleteHenCommand(IMessageBroker messageBroker, IHenService henService)
            : base(messageBroker)
        {
            this.henService = henService;
            this.messageBroker = messageBroker;
        }

        protected override void OnDeleteMasterData(Guid entityId)
        {
            if (MessageBox.Show(LanguageData.General_DeleteConfirmation, LanguageData.General_Delete, MessageBoxButton.YesNo)
                == MessageBoxResult.No)
                return;

            messageBroker.Publish(
              henService.Delete(entityId) ? CommonMessages.DeleteHouseSuccess : CommonMessages.DeleteHouseFailed,
              entityId);
        }
    }
}
