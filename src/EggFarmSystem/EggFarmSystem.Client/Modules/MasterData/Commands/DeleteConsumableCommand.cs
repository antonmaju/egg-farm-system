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
    public class DeleteConsumableCommand : DeleteMasterDataCommand
    {
        private readonly IConsumableService consumableService;

        public DeleteConsumableCommand(IMessageBroker messageBroker, IConsumableService consumableService)
            :base(messageBroker)
        {
            this.consumableService = consumableService;
        }

        protected override void OnDeleteMasterData(Guid entityId)
        {
            if(MessageBox.Show(LanguageData.General_DeleteConfirmation, LanguageData.General_Delete,MessageBoxButton.YesNo)
                == MessageBoxResult.No)
                return;

            consumableService.Delete(entityId);
        }
    }
}
