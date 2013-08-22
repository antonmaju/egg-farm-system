using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.EggProduction.Views;
using EggFarmSystem.Resources;

namespace EggFarmSystem.Client.Modules.EggProduction.Commands
{
    public class EditEggProductionCommand : CommandBase
    {
        private readonly IMessageBroker broker;

        public EditEggProductionCommand(IMessageBroker broker)
        {
            Text = () => LanguageData.General_Edit;

            this.broker = broker;
        }

        public Guid EntityId { get; set; }

        public override bool CanExecute(object parameter)
        {
            try
            {
                Guid entityId = parameter != null ? (Guid)parameter : EntityId;
                return entityId != Guid.Empty;
            }
            catch
            {
                return false;
            }
        }

        public override void Execute(object parameter)
        {
            Guid entityId = parameter != null ? (Guid)parameter : EntityId;
            broker.Publish(CommonMessages.ChangeMainView, typeof(IEggProductionEntryView));
            broker.Publish(CommonMessages.LoadEggProduction, entityId);
        }
    }
}
