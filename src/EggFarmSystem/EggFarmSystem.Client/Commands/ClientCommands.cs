using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace EggFarmSystem.Client.Commands
{
    public static class ClientCommands
    {
        static ClientCommands()
        {
            Minimize = new RoutedUICommand("Minimize", "Minimize", typeof(Window));
            Maximize = new RoutedUICommand("Maximize", "Maximize", typeof(Window));

        }

        public static RoutedUICommand Minimize { get; private set; }

        public static RoutedUICommand Maximize { get; private set; }
    }
}
