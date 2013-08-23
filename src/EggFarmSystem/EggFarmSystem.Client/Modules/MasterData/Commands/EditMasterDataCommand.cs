using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.MasterData.Views;
using EggFarmSystem.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Modules.MasterData.Commands
{
    public abstract class EditMasterDataCommand : CommandBase
    {
        private readonly IMessageBroker messageBroker;
        private readonly IClientContext clientContext;

        protected EditMasterDataCommand(IMessageBroker messageBroker, IClientContext clientContext)
        {
            Text = () => LanguageData.General_Edit;

            this.messageBroker = messageBroker;
            this.clientContext = clientContext;
        }

        public Guid EntityId { get; set; }

        public override void Execute(object parameter)
        {
            if (clientContext.MainViewType != typeof(IMasterDataView))
                messageBroker.Publish(CommonMessages.ChangeMainView, typeof(IMasterDataView));

            Guid entityId = parameter != null ? (Guid)parameter : EntityId;

            if (entityId == Guid.Empty)
                return;

            OnExecute(entityId);
        }

        protected abstract void OnExecute(Guid id);

        public override bool CanExecute(object parameter)
        {
            if (EntityId != Guid.Empty)
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
