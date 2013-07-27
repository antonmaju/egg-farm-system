using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Resources;
using EggFarmSystem.Services;

namespace EggFarmSystem.Client.Modules.EmployeeCost.Commands
{
    public class SaveEmployeeCostCommand : CommandBase
    {
        private readonly IMessageBroker broker;
        private readonly IEmployeeCostService costService;

        public SaveEmployeeCostCommand(IMessageBroker broker, IEmployeeCostService costService)
        {
            Text = () => LanguageData.General_Save;

            this.broker = broker;
            this.costService = costService;
        }

        public Models.EmployeeCost Cost { get; set; }

        public override bool CanExecute(object parameter)
        {
            return Cost != null;
        }

        public override void Execute(object parameter)
        {
            try
            {
                costService.Save(Cost);
                broker.Publish(CommonMessages.SaveEmployeeCostSuccess, Cost);
            }
            catch(Exception ex)
            {
                broker.Publish(CommonMessages.SaveEmployeeCostFailed, new Error
                    {
                        Data = Cost,
                        Exception = ex
                    });
            }
        }
    }
}
