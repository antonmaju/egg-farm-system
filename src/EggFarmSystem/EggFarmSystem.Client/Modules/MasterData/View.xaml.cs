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

namespace EggFarmSystem.Client.Modules.MasterData
{
    /// <summary>
    /// Interaction logic for View.xaml
    /// </summary>
    public partial class View : UserControl
    {
        public View()
        {
            InitializeComponent();

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
}
