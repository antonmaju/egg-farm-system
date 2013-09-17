using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Models;
using EggFarmSystem.Resources;
using EggFarmSystem.Services;

namespace EggFarmSystem.Client.Modules.Usage.Commands
{
    public class SaveUsageCommand : CommandBase
    {
        private readonly IMessageBroker messageBroker;
        private readonly IConsumableUsageService usageService;

        public SaveUsageCommand(IMessageBroker messageBroker, IConsumableUsageService usageService)
        {
            Text = () => LanguageData.General_Save;

            this.messageBroker = messageBroker;
            this.usageService = usageService;
        }

        public ConsumableUsage Usage { get; set; }

        public override bool CanExecute(object parameter)
        {
            return Usage != null;
        }

        public override void Execute(object parameter)
        {
            try
            {
                usageService.Save(Usage);
                messageBroker.Publish(CommonMessages.SaveUsageSuccess, Usage);
            }
            catch(Exception ex)
            {
                messageBroker.Publish(CommonMessages.SaveUsageFailed, new Error
                    {
                        Data = Usage,
                        Exception = ex
                    });
            }
        }
    }
}
