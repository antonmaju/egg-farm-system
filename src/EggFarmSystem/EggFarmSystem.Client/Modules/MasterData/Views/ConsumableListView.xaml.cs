using EggFarmSystem.Client.Core.Views;
using EggFarmSystem.Client.Modules.MasterData.ViewModels;
using EggFarmSystem.Models;
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
    /// Interaction logic for ConsumableListView.xaml
    /// </summary>
    public partial class ConsumableListView : UserControlBase, IConsumableListView
    {
        private ConsumableListViewModel viewModel;

        public ConsumableListView(ConsumableListViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.DataContext = viewModel;
            this.NavigationCommands = viewModel.NavigationCommands;
            SetEventHandlers();
        }

        void SetEventHandlers()
        {
            lvConsumableList.MouseUp += lvConsumableList_MouseUp;
            lvConsumableList.MouseDoubleClick += lvConsumableList_MouseDoubleClick;
        }

        void lvConsumableList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            viewModel.EditCommand.Execute(null);
        }

        private void lvConsumableList_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var selectedConsumable = lvConsumableList.SelectedItem as Consumable;
            if (selectedConsumable == null)
                return;

            viewModel.EditCommand.EntityId = selectedConsumable.Id;
            viewModel.DeleteCommand.EntityId = selectedConsumable.Id;
        }

        void UnsetEventHandlers()
        {
            lvConsumableList.MouseUp -= lvConsumableList_MouseUp;
            lvConsumableList.MouseDoubleClick -= lvConsumableList_MouseDoubleClick;
        }

        public override void Dispose()
        {
            UnsetEventHandlers();
            base.Dispose();
        }
    }

    public interface IConsumableListView
    {
        
    }
}
