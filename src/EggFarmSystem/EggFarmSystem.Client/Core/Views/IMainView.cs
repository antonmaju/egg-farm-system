using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using EggFarmSystem.Client.Commands;

namespace EggFarmSystem.Client.Core.Views
{
    public interface IMainView
    {
        void Initialize();

        void ChangeView(UserControlBase newView);

        void ChangeActionCommands(IList<CommandBase> commands);
    }
}
