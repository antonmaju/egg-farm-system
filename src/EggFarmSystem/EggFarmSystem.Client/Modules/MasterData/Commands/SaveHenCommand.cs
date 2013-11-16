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
            var hen = parameter as Hen ?? Hen;

            if (hen == null)
                return;

            try
            {
                henService.Save(hen);
                messageBroker.Publish(CommonMessages.HenSaved, hen);
            }
            catch (Exception ex)
            {
                var error = new Error(ex, hen);
                messageBroker.Publish(CommonMessages.HenSavingFailed, error);
            }
        }
    }
}
