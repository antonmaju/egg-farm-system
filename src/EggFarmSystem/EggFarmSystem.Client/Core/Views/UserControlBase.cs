using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using EggFarmSystem.Client.Commands;

namespace EggFarmSystem.Client.Core.Views
{
    public abstract class UserControlBase : UserControl, IDisposable
    {

        public IList<CommandBase> NavigationCommands { get; set; }

        public virtual void Dispose()
        {
            
        }
    }
}
