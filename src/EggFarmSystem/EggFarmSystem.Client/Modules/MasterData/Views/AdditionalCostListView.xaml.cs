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
using EggFarmSystem.Client.Modules.MasterData.ViewModels;
using EggFarmSystem.Models;

namespace EggFarmSystem.Client.Modules.MasterData.Views
{
    /// <summary>
    /// Interaction logic for AdditionalCostListView.xaml
    /// </summary>
    public partial class AdditionalCostListView : UserControlBase, IAdditionalCostListView
    {
        private AdditionalCostListViewModel viewModel;

        public AdditionalCostListView(AdditionalCostListViewModel model)
        {
            InitializeComponent();
            this.DataContext = model;
            this.viewModel = model;
            this.NavigationCommands = viewModel.NavigationCommands;
            SetEventHandlers();
        }

        void SetEventHandlers()
        {
            lvCostList.MouseUp +=lvCostList_MouseUp;
            lvCostList.MouseDoubleClick += lvCostList_MouseDoubleClick;
        }

        void lvCostList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            viewModel.EditCommand.Execute(null);
        }

        void lvCostList_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var selectedCost = lvCostList.SelectedItem as AdditionalCost;
            if (selectedCost == null)
                return;

            viewModel.EditCommand.EntityId = selectedCost.Id;
            viewModel.DeleteCommand.EntityId = selectedCost.Id;
        }

        void UnsetEventHandlers()
        {
            lvCostList.MouseUp -= lvCostList_MouseUp;
            lvCostList.MouseDoubleClick += lvCostList_MouseDoubleClick;
        }

        public override void Dispose()
        {
            UnsetEventHandlers();
            base.Dispose();
        }

    }

    public interface IAdditionalCostListView
    {
        
    }
}
