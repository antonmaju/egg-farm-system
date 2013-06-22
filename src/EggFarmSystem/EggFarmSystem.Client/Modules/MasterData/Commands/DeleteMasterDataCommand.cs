using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Modules.MasterData.Commands
{
    public abstract class DeleteMasterDataCommand : CommandBase
    {
        private readonly IMessageBroker messageBroker;

        protected DeleteMasterDataCommand(IMessageBroker messageBroker)
        {
            Text = () => LanguageData.General_Delete;
            this.messageBroker = messageBroker;
        }

        public Guid EntityId { get; set; }

        public override void Execute(object parameter)
        {
            Guid id = Guid.Empty;
            try
            {
                id = parameter == null ? EntityId : (Guid)parameter;
                OnDeleteMasterData(id);       
            }
            catch (Exception ex)
            {
                var error = new Error {Exception = ex, Data = id};
            }
        }

        protected abstract void OnDeleteMasterData(Guid entityId);

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
