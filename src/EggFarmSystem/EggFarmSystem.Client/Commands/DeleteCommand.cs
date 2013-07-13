using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using EggFarmSystem.Resources;

namespace EggFarmSystem.Client.Commands
{
    public abstract class DeleteCommand : CommandBase
    {
        protected DeleteCommand()
        {
            Text = () => LanguageData.General_Delete;
        }

        public Guid EntityId { get; set; }

        public override bool CanExecute(object parameter)
        {
            if (EntityId != Guid.Empty)
                return true;

            if (parameter == null)
                return false;

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

        public override void Execute(object parameter)
        {
            if (MessageBox.Show(LanguageData.General_DeleteConfirmation, LanguageData.General_Delete, MessageBoxButton.YesNo)
               == MessageBoxResult.No)
                return;

            Guid id = Guid.Empty;

            try
            {
                id = parameter == null ? EntityId : (Guid)parameter;
                OnDeleteData(id);       
            }
            catch(Exception exception)
            {
                
            }
        }

        protected abstract void OnDeleteData(Guid entityId);

        
    }
}
