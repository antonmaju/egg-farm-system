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
    /// Interaction logic for HenListView.xaml
    /// </summary>
    public partial class HenListView : UserControlBase, IHenListView
    {

        private HenListViewModel ViewModel { get; set; }

        public HenListView(HenListViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            this.DataContext = ViewModel;
            this.NavigationCommands = viewModel.NavigationCommands;
            SetDataBindings();
        }

        private void SetDataBindings()
        {
            lvHenList.MouseDoubleClick += lvHenList_MouseDoubleClick;
        }

        void lvHenList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedHen = lvHenList.SelectedItem as Hen;
            ViewModel.EditHenCommand.Execute(selectedHen.Id);
        }

        private void UnsetDataBindings()
        {
            lvHenList.MouseDoubleClick -= lvHenList_MouseDoubleClick;  
        }

        public override void Dispose()
        {
            UnsetDataBindings();
        }
    }

    public interface IHenListView
    {
        
    }
}
