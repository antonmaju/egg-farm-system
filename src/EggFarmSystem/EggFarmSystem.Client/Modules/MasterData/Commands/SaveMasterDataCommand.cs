using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Models;
using EggFarmSystem.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Modules.MasterData.Commands
{
    public abstract class SaveMasterDataCommand<T> : CommandBase where T:Entity
    {
        private readonly IMessageBroker messageBroker;
    
        protected SaveMasterDataCommand(IMessageBroker messageBroker)
        {
            Text = () => LanguageData.General_Save;

            this.messageBroker = messageBroker;
        }

        public T Entity { get; set; }

        public override bool CanExecute(object parameter)
        {
            return Entity != null;
        }

        public override void Execute(object parameter)
        {
            T entity = parameter as T;

            if (entity == null)
                entity = Entity;

            if (entity == null)
                return;

            OnSave(entity) ;
        }

        protected abstract void OnSave(T entity);
    }
}
