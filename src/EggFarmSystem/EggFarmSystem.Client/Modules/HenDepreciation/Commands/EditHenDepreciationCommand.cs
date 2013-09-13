using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Resources;

namespace EggFarmSystem.Client.Modules.HenDepreciation.Commands
{
    public class EditHenDepreciationCommand : CommandBase
    {
        private readonly IMessageBroker broker;

        public EditHenDepreciationCommand(IMessageBroker broker)
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
            if (entityId == Guid.Empty)
                return;

            //broker.Publish(CommonMessages.ChangeMainView, typeof(IHenDepreciationEntryView));
            broker.Publish(CommonMessages.LoadHenDepreciation, entityId);
        }

    }
}
