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
    /// Interaction logic for HenHouseListView.xaml
    /// </summary>
    public partial class HenHouseListView : UserControlBase, IHenHouseListView
    {
        private readonly HouseListViewModel viewModel;

        public HenHouseListView(HouseListViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.DataContext = viewModel;
            this.NavigationCommands = viewModel.NavigationCommands;
            SetEventHandlers();
        }

        void SetEventHandlers()
        {
            lvHouseList.MouseUp += lvHouseList_MouseDown;
            lvHouseList.MouseDoubleClick += lvHouseList_MouseDoubleClick;
        }

        void lvHouseList_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var selectedHouse = lvHouseList.SelectedItem as HenHouse;
            if (selectedHouse == null)
                return;

            viewModel.EditCommand.HouseId = selectedHouse.Id;
            //viewModel.DeleteCommand.Id = selectedHouse.Id;
        }

        void lvHouseList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            viewModel.EditCommand.Execute(null);
        }

        void UnsetEventHandlers()
        {
            lvHouseList.MouseUp -= lvHouseList_MouseDown;
            lvHouseList.MouseDoubleClick -= lvHouseList_MouseDoubleClick;
        }

        public override void Dispose()
        {
            UnsetEventHandlers();
            base.Dispose();
        }
    }

    public interface IHenHouseListView
    {
        
    }
}
