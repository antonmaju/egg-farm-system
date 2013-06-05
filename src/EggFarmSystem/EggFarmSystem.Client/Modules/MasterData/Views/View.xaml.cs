using System.Windows;
using System.Windows.Controls;
using EggFarmSystem.Client.Modules.MasterData.ViewModels;

namespace EggFarmSystem.Client.Modules.MasterData.Views
{
    /// <summary>
    /// Interaction logic for View.xaml
    /// </summary>
    public partial class View : UserControl, IMasterDataView
    {
        public View(MasterDataViewModel viewModel)
        {
            InitializeComponent();
            //this.DataContext = viewModel;
        }

        private void tglKandang_Click(object sender, RoutedEventArgs e)
        {
            scrMasterContent.Content = new HenHouseForm();
        }

        private void tglAyam_Click(object sender, RoutedEventArgs e)
        {
            scrMasterContent.Content = new HenForm();
        }

        private void tglKaryawan_Click(object sender, RoutedEventArgs e)
        {
            scrMasterContent.Content = new EmployeeForm();
        }
    }

    public interface IMasterDataView
    {
        
    }
}
