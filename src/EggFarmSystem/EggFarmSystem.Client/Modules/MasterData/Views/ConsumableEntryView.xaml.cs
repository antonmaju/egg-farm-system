using EggFarmSystem.Client.Core.Views;
using EggFarmSystem.Client.Modules.MasterData.ViewModels;
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

namespace EggFarmSystem.Client.Modules.MasterData.Views
{
    /// <summary>
    /// Interaction logic for ConsumableEntryView.xaml
    /// </summary>
    public partial class ConsumableEntryView : UserControlBase, IConsumableEntryView
    {
        private readonly ConsumableEntryViewModel viewModel;

        public ConsumableEntryView(ConsumableEntryViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            this.NavigationCommands = viewModel.NavigationCommands;
        }

    }

    public interface IConsumableEntryView
    {
        
    }
}
