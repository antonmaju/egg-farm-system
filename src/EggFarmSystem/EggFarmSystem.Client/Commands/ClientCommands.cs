using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace EggFarmSystem.Client.Commands
{
    /// <summary>
    /// Contains UI commands for main window 
    /// </summary>
    public static class ClientCommands
    {
        static ClientCommands()
        {
            Minimize = new RoutedUICommand("Minimize", "Minimize", typeof(Window));
            Maximize = new RoutedUICommand("Maximize", "Maximize", typeof(Window));
        }

        /// <summary>
        /// Gets the minimize window command.
        /// </summary>
        /// <value>The minimize command.</value>
        public static RoutedUICommand Minimize { get; private set; }

        /// <summary>
        /// Gets the maximize window command.
        /// </summary>
        /// <value>The maximize command.</value>
        public static RoutedUICommand Maximize { get; private set; }
    }
}
