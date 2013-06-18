using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Models;
using EggFarmSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Modules.MasterData.Commands
{
    public class SaveHenHouseCommand :CommandBase
    {
        private readonly IHenHouseService houseService;
        private readonly IMessageBroker messageBroker;

        public SaveHenHouseCommand(IMessageBroker messageBroker, IHenHouseService houseService)
        {
            Text = () => "Save";
            this.messageBroker = messageBroker;
            this.houseService = houseService;
        }

        public HenHouse HenHouse { get; set; }

        public override void Execute(object parameter)
        {
            if (HenHouse == null)
                return;

            try
            {
                if (houseService.Save(HenHouse))
                {
                    messageBroker.Publish(CommonMessages.SaveHouseSuccess, null);
                }
                else
                {
                    messageBroker.Publish(CommonMessages.SaveHouseFailed, null);
                }
            }
            catch(Exception ex)
            {
                messageBroker.Publish(CommonMessages.SaveHouseFailed, ex.Message);
            }
        }
    }
}
