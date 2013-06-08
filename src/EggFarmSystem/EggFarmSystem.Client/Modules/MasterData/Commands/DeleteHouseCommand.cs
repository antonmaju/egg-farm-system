using EggFarmSystem.Client.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Modules.MasterData.Commands
{
    public class DeleteHouseCommand : CommandBase 
    {
        public DeleteHouseCommand()
        {
            Text = () => "Delete";
        }

        public override void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
