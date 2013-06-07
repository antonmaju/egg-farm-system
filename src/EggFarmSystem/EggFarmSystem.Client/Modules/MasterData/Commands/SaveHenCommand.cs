using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Models;
using EggFarmSystem.Services;

namespace EggFarmSystem.Client.Modules.MasterData.Commands
{
    public class SaveHenCommand : CommandBase
    {
        private IHenService henService;
        private IMessageBroker messageBroker;

        public SaveHenCommand(IMessageBroker messageBroker, IHenService henService)
        {
            Text = () => "Save";
            this.henService = henService;
            this.messageBroker = messageBroker;
        }

        public Hen Hen { get; set; }

        public override void Execute(object parameter)
        {
            if (Hen == null)
                return;

            try
            {
                henService.Save(Hen);
                messageBroker.Publish(CommonMessages.HenSaved, null);
            }
            catch (Exception ex)
            {
                messageBroker.Publish(CommonMessages.HenSavingFailed, ex.Message);
            }
        }
    }
}
