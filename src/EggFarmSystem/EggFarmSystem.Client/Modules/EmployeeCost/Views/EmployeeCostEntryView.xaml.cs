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
using EggFarmSystem.Client.Modules.EmployeeCost.ViewModels;

namespace EggFarmSystem.Client.Modules.EmployeeCost.Views
{
    /// <summary>
    /// Interaction logic for EmployeeCostEntryView.xaml
    /// </summary>
    public partial class EmployeeCostEntryView : UserControlBase, IEmployeeCostEntryView
    {
        private EmployeeCostEntryViewModel model;

        public EmployeeCostEntryView(EmployeeCostEntryViewModel model)
        {
            InitializeComponent();
            this.model = model;
            this.DataContext = model;
            this.NavigationCommands = model.NavigationCommands;
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            dgCost.SelectionChanged += dgCost_SelectionChanged;
        }

        void dgCost_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var grid = sender as DataGrid;
            int index = grid.Items.IndexOf(grid.SelectedItem);
            model.DeleteDetailCommand.Tag = index;
        }

        private void UnsubscribeEvents()
        {
            dgCost.SelectionChanged -= dgCost_SelectionChanged;
        }

        public override void Dispose()
        {
            UnsubscribeEvents();
            model.Dispose();
            base.Dispose();
        }
    }

    public interface IEmployeeCostEntryView
    {
        
    }
}
