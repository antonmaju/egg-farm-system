using System.Windows;
using EggFarmSystem.Client.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Resources;
using EggFarmSystem.Services;

namespace EggFarmSystem.Client.Modules.MasterData.Commands
{
    public class DeleteHouseCommand : DeleteMasterDataCommand
    {
        private readonly IHenHouseService houseService;
        private readonly IMessageBroker messageBroker;

        public DeleteHouseCommand(IMessageBroker messageBroker, IHenHouseService houseService)
            :base(messageBroker)
        {
            this.houseService = houseService;
            this.messageBroker = messageBroker;
        }

        protected override void OnDeleteMasterData(Guid entityId)
        {
            if (MessageBox.Show(LanguageData.General_DeleteConfirmation, LanguageData.General_Delete, MessageBoxButton.YesNo)
                == MessageBoxResult.No)
                return;

            messageBroker.Publish(
                houseService.Delete(entityId) ? CommonMessages.DeleteHouseSuccess : CommonMessages.DeleteHouseFailed,
                entityId);
        }

       
    }
}
