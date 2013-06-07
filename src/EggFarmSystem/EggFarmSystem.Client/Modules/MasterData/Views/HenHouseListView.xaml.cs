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

namespace EggFarmSystem.Client.Modules.MasterData.Views
{
    /// <summary>
    /// Interaction logic for HenHouseListView.xaml
    /// </summary>
    public partial class HenHouseListView : UserControlBase, IHenHouseListView
    {
        public HenHouseListView()
        {
            InitializeComponent();
        }
    }

    public interface IHenHouseListView
    {
        
    }
}
