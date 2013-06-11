using EggFarmSystem.Client.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Services;

namespace EggFarmSystem.Client.Modules.MasterData.Commands
{
    public class DeleteHouseCommand : CommandBase
    {
        private readonly IMessageBroker messageBroker;
        private readonly IHenHouseService houseService;

        public DeleteHouseCommand(IMessageBroker messageBroker, IHenHouseService houseService)
        {
            Text = () => "Delete";

            this.messageBroker = messageBroker;
            this.houseService = houseService;
        }

        public Guid HouseId { get; set; }

        public override void Execute(object parameter)
        {
            Guid id = parameter == null ? HouseId : (Guid)parameter;

            try
            {
                houseService.Delete(id);
                messageBroker.Publish(CommonMessages.DeleteHouseSuccess, id);
            }
            catch (Exception ex)
            {
                messageBroker.Publish(CommonMessages.DeleteHouseFailed, ex.Message);
            }
        }
    }
}
