using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.MasterData.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Modules.MasterData.Commands
{
    public class EditEmployeeCommand : CommandBase
    {
        private readonly IMessageBroker messageBroker;
        private readonly IClientContext clientContext;

        public EditEmployeeCommand(IMessageBroker messageBroker, IClientContext clientContext)
        {
            Text = () => "Edit";
            this.messageBroker = messageBroker;
            this.clientContext = clientContext;
        }

        public Guid EmployeeId { get; set; }

        public override void Execute(object parameter)
        {
            if (clientContext.MainViewType != typeof(IMasterDataView))
                messageBroker.Publish(CommonMessages.ChangeMainView, typeof(IMasterDataView));

            Guid employeeId = parameter != null ? (Guid)parameter : EmployeeId;

            messageBroker.Publish(CommonMessages.EditEmployeeView, employeeId);
        }

        public override bool CanExecute(object parameter)
        {
            if (EmployeeId != Guid.Empty)
                return true;

            try
            {
                var paramId = (Guid)parameter;
                return paramId != Guid.Empty;
            }
            catch
            {
                return false;
            }
        }
    }
}
