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
using EggFarmSystem.Client.Modules.EmployeeCost.ViewModels;

namespace EggFarmSystem.Client.Modules.EmployeeCost.Views
{
    /// <summary>
    /// Interaction logic for EmployeeCostLIstView.xaml
    /// </summary>
    public partial class EmployeeCostListView : UserControlBase, IEmployeeCostListView
    {
        private readonly EmployeeCostListViewModel model;

        public EmployeeCostListView(EmployeeCostListViewModel model)
        {
            InitializeComponent();

            this.model = model;
            this.DataContext = model;
            this.NavigationCommands = model.NavigationCommands;

            SetEventHandlers();
        }

        void SetEventHandlers()
        {
            lvCostList.MouseUp += lvCostList_MouseUp;
            lvCostList.MouseDoubleClick += lvCostList_MouseDoubleClick;
            pager.PageIndexChanged += pager_PageIndexChanged;
        }

        void lvCostList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            model.EditCommand.Execute(null);
        }

        void lvCostList_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var selectedCost = lvCostList.SelectedItem as Models.EmployeeCost;
            if (selectedCost == null)
                return;

            model.EditCommand.EntityId = selectedCost.Id;
            model.DeleteCommand.EntityId = selectedCost.Id;
        }

        void pager_PageIndexChanged(object sender, Controls.PagerEventArgs args)
        {
            model.PageIndex = args.PageIndex;
        }

        void UnsetEventHandlers()
        {
            lvCostList.MouseUp -= lvCostList_MouseUp;
            lvCostList.MouseDoubleClick -= lvCostList_MouseDoubleClick;
            pager.PageIndexChanged -= pager_PageIndexChanged;
        }

        public override void Dispose()
        {
            UnsetEventHandlers();
            model.Dispose();
            base.Dispose();
        }

    }

    public interface IEmployeeCostListView
    {

    }
}
