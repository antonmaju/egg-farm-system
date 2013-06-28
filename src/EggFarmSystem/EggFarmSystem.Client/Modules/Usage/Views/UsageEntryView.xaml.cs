using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EggFarmSystem.Client.Core.Views;
using EggFarmSystem.Client.Modules.Usage.ViewModels;

namespace EggFarmSystem.Client.Modules.Usage.Views
{
    /// <summary>
    /// Interaction logic for UsageEntryView.xaml
    /// </summary>
    public partial class UsageEntryView : UserControlBase, IUsageEntryView
    {
        public UsageEntryView(UsageEntryViewModel model)
        {
            InitializeComponent();
            this.DataContext = model;
            this.NavigationCommands = model.NavigationCommands;
        }
    }

    public interface  IUsageEntryView
    {
        
    }
}
