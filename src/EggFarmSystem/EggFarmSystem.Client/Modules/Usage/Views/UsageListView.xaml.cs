using EggFarmSystem.Client.Core.Views;
using EggFarmSystem.Client.Modules.Usage.ViewModels;
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
using EggFarmSystem.Models;

namespace EggFarmSystem.Client.Modules.Usage.Views
{
    /// <summary>
    /// Interaction logic for UsageListView.xaml
    /// </summary>
    public partial class UsageListView : UserControlBase, IUsageListView
    {
        private UsageListViewModel model;

        public UsageListView(UsageListViewModel model)
        {
            InitializeComponent();

            this.model = model;
            this.DataContext = model;
            this.NavigationCommands = model.NavigationCommands;

            SetEventHandlers();
        }

        private void SetEventHandlers()
        {
            lvUsageList.MouseUp += new MouseButtonEventHandler(lvUsageList_MouseUp);
            lvUsageList.MouseDoubleClick += new MouseButtonEventHandler(lvUsageList_MouseDoubleClick);
        }

        void lvUsageList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            model.EditCommand.Execute(null);
        }

        void lvUsageList_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var selectedUsage = lvUsageList.SelectedItem as ConsumableUsage;
            if (selectedUsage == null)
                return;

            model.EditCommand.EntityId = selectedUsage.Id;
            model.DeleteCommand.EntityId = selectedUsage.Id;
        }

        private void UnsetEventHandlers()
        {
            lvUsageList.MouseUp -= new MouseButtonEventHandler(lvUsageList_MouseUp);
            lvUsageList.MouseDoubleClick -= new MouseButtonEventHandler(lvUsageList_MouseDoubleClick);
        }

        public override void Dispose()
        {
            UnsetEventHandlers();
            base.Dispose();
        }
    }

    public interface IUsageListView
    {
        
    }
}
