using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.EmployeeCost.Views;
using EggFarmSystem.Resources;

namespace EggFarmSystem.Client.Modules.EmployeeCost.Commands
{
    public class EditEmployeeCostCommand : CommandBase
    {
        private readonly IMessageBroker messageBroker;

        public EditEmployeeCostCommand(IMessageBroker messageBroker)
        {
            Text = () => LanguageData.General_Edit;

            this.messageBroker = messageBroker;
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
            Guid entityId = parameter != null ? (Guid) parameter : EntityId;
            messageBroker.Publish(CommonMessages.ChangeMainView, typeof(IEmployeeCostEntryView));
            messageBroker.Publish(CommonMessages.LoadEmployeeCost, entityId);
        }
    }
}
