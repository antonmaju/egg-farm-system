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
    public class SaveHenDepreciationCommand : CommandBase
    {
        private readonly IMessageBroker broker;
        private readonly IHenDepreciationService service;

        public SaveHenDepreciationCommand(IMessageBroker broker, IHenDepreciationService service)
        {
            Text = () => LanguageData.General_Save;

            this.broker = broker;
            this.service = service;
        }

        public Models.HenDepreciation Depreciation { get; set; }

        public override bool CanExecute(object parameter)
        {
            return Depreciation != null && !Depreciation.Validate().Any();
        }

        public override void Execute(object parameter)
        {
            try
            {
                service.Save(Depreciation);
                broker.Publish(CommonMessages.SaveHenDepreciationSuccess, Depreciation);
            }
            catch (Exception ex)
            {
                broker.Publish(CommonMessages.SaveHenDepreciationFailed, new Error
                    {
                        Data = Depreciation,
                        Exception = ex
                    });
            }
        }
    }
}
