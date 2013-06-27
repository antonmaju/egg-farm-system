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
            pager.PagingSource = model;
        }

    }

    public interface IUsageListView
    {
        
    }
}
