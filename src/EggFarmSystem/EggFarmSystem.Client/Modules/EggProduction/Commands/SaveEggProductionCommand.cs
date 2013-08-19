using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Resources;
using EggFarmSystem.Services;

namespace EggFarmSystem.Client.Modules.EggProduction.Commands
{
    public class SaveEggProductionCommand : CommandBase
    {
        private readonly IMessageBroker broker;
        private readonly IEggProductionService service;

        public SaveEggProductionCommand(IMessageBroker broker, IEggProductionService service)
        {
            Text = () => LanguageData.General_Save;

            this.broker = broker;
            this.service= service;
        }

        public Models.EggProduction Production { get; set; }

        public override bool CanExecute(object parameter)
        {
            return Production != null;
        }

        public override void Execute(object parameter)
        {
            try
            {
                service.Save(Production);
                broker.Publish(CommonMessages.SaveEggProductionSuccess, Production);
            }
            catch(Exception ex)
            {
                broker.Publish(CommonMessages.SaveEggProductionSuccess, new Error
                    {
                        Data = Production,
                        Exception = ex
                    });
            }
        }

    }
}
