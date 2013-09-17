using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Resources;
using EggFarmSystem.Services;

namespace EggFarmSystem.Client.Modules.HenDepreciation.Commands
{
    public class DeleteHenDepreciationCommand : DeleteCommand
    {
        private readonly IMessageBroker broker;
        private readonly IHenDepreciationService service;

        public DeleteHenDepreciationCommand(IMessageBroker broker, IHenDepreciationService depreciationService)
        {
            Text = () => LanguageData.General_Delete;

            this.broker = broker;
            this.service = depreciationService;
        }

        protected override void OnDeleteData(Guid entityId)
        {
            try
            {
                service.Delete(entityId);
                broker.Publish(CommonMessages.DeleteHenDepreciationSuccess, entityId);
            }
            catch (Exception ex)
            {
                var error = new Error(ex, entityId);
                broker.Publish(CommonMessages.DeleteHenDepreciationFailed, error);
            }
        }
    }
}
