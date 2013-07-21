using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.EmployeeCost.Commands;

namespace EggFarmSystem.Client.Modules.EmployeeCost.ViewModels
{
    public class EmployeeCostEntryViewModel : ViewModelBase
    {
        public EmployeeCostEntryViewModel(CancelCommand cancelCommand, ShowEmployeeCostCommand showListCommand)
        {
            CancelCommand = cancelCommand;
            NavigationCommands = new List<CommandBase>{CancelCommand};
            CancelCommand.Action = broker => showListCommand.Execute(null);
        }

        #region commands

        public CancelCommand CancelCommand { get; private set; }

        public IList<CommandBase> NavigationCommands { get; private set; }

        #endregion
    }
}
