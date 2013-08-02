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
        }

        
    }

    public interface IAdditionalCostListView
    {
        
    }
}
