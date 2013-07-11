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
        private UsageEntryViewModel model;

        public UsageEntryView(UsageEntryViewModel model)
        {
            InitializeComponent();
            this.model = model;
            this.DataContext = model;
            this.NavigationCommands = model.NavigationCommands;
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            dgUsage.CellEditEnding += dgUsage_CellEditEnding;
            dgUsage.GotFocus += dgUsage_GotFocus;
            dgUsage.SelectionChanged += dgUsage_SelectionChanged;
        }

        void dgUsage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var grid = sender as DataGrid;
            int index = grid.Items.IndexOf(grid.SelectedItem);
            model.DeleteDetailCommand.Tag = index;
        }

        void dgUsage_GotFocus(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(DataGridCell))
            {
                var grd = (DataGrid)sender;
                grd.BeginEdit(e);
            }
        }

        private bool isManualEditCommit;
        void dgUsage_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (!isManualEditCommit)
            {
                isManualEditCommit = true;
                var grid = (DataGrid)sender;
                grid.CommitEdit(DataGridEditingUnit.Row, true);
                isManualEditCommit = false;
            }
        }
    }

    public interface  IUsageEntryView
    {
        
    }
}
