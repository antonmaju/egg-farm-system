using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.MasterData.Commands;
using EggFarmSystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Modules.MasterData.ViewModels
{
    public class HenListViewModel : ViewModelBase
    {
        private readonly IMessageBroker messageBroker;
        
        public HenListViewModel(IMessageBroker messageBroker, NewHenCommand newHenCommand)
        {
            this.messageBroker = new MessageBroker();
            NewHenCommand = newHenCommand;

            var data = new List<Hen>
                {
                    new Hen {Active = true, Count = 1, Id = Guid.NewGuid(), Name = "Test", Type = "Jago"},
                    new Hen {Active = true, Count = 10, Id = Guid.NewGuid(), Name = "Test 2", Type = "Jago"}
                };
            Hens = new ObservableCollection<Hen>(data);

            NavigationCommands = new List<CommandBase>() {NewHenCommand};
        }

        public CommandBase NewHenCommand { get; private set; }

        public ObservableCollection<Hen> Hens { get; private set; }

        public IList<CommandBase> NavigationCommands { get; private set; }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
