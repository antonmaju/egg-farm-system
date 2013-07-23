using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.EmployeeCost.Commands;
using EggFarmSystem.Resources;

namespace EggFarmSystem.Client.Modules.EmployeeCost.ViewModels
{
    public class EmployeeCostEntryViewModel : ViewModelBase
    {
        private readonly IMessageBroker messageBroker;

        public EmployeeCostEntryViewModel(
            IMessageBroker messageBroker, 
            CancelCommand cancelCommand, ShowEmployeeCostCommand showListCommand)
        {
            this.messageBroker = messageBroker;

            CancelCommand = cancelCommand;
            NavigationCommands = new List<CommandBase>{CancelCommand};
            CancelCommand.Action = broker => showListCommand.Execute(null);
        }

        #region commands

        public DelegateCommand AddDetailCommand { get; private set; }
        public DelegateCommand<int> DeleteDetailCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }
        private SaveEmployeeCostCommand ActualSaveCommand { get; set; }
        public ShowEmployeeCostCommand ShowCostListCommand { get; private set; }
        
        public CancelCommand CancelCommand { get; private set; }
        public IList<CommandBase> NavigationCommands { get; private set; }

        private void InitializeCommands()
        {
            SaveCommand = new DelegateCommand(Save, CanSave) {Text = () => LanguageData.General_Save};

            AddDetailCommand = new DelegateCommand(AddDetail, CanAddDetail) {Text =() => LanguageData.General_New};

            DeleteDetailCommand = new DelegateCommand<int>(DeleteDetail, CanDeleteDetail) { Text = () => LanguageData.General_Delete };
        }

        void Save(object param)
        {
            
        }

        bool CanSave(object param)
        {
            return false;
        }

        void AddDetail(object param)
        {
            
        }

        bool CanAddDetail(object param)
        {
            return true;
        }

        void DeleteDetail(int param)
        {
           
        }

        bool CanDeleteDetail(int param)
        {
            return true;
        }

        
        #endregion

       
    }
}
