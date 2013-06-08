using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Modules.MasterData.Commands
{
    public class DeleteHenCommand : CommandBase
    {
        private readonly IMessageBroker messageBroker;
        private readonly IHenService henService;

        public DeleteHenCommand(
            IMessageBroker messageBroker,
            IHenService henService)
        {
            Text = () => "Delete";
            this.messageBroker = messageBroker;
            this.henService = henService;
        }

        public Guid Id { get; set; }

        public override void Execute(object parameter)
        {
            Guid id = parameter == null ? Id : (Guid) parameter;
            
            try
            {
                henService.Delete(id);
                messageBroker.Publish(CommonMessages.DeleteHenSuccess, id);
            }
            catch(Exception ex)
            {
                messageBroker.Publish(CommonMessages.DeleteHenFailed, ex.Message);
            }
        }
    }
}
