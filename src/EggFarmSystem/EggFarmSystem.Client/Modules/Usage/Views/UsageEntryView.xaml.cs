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
            dgUsage.SelectionChanged += dgUsage_SelectionChanged;
        }

        void dgUsage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var grid = sender as DataGrid;
            int index = grid.Items.IndexOf(grid.SelectedItem);
            model.DeleteDetailCommand.Tag = index;
        }

        public void UnsubscribeEvents()
        {
            dgUsage.SelectionChanged -= dgUsage_SelectionChanged;
        }

        public override void Dispose()
        {
            UnsubscribeEvents();
            model.Dispose();
            base.Dispose();
        }
    }

    public interface  IUsageEntryView
    {
        
    }
}
