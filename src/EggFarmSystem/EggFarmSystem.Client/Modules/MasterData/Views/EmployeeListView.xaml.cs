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
    /// Interaction logic for EmployeeListView.xaml
    /// </summary>
    public partial class EmployeeListView : UserControlBase, IEmployeeListView
    {
        private EmployeeListViewModel viewModel;

        public EmployeeListView(EmployeeListViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.DataContext = viewModel;
            this.NavigationCommands = viewModel.NavigationCommands;
            SetEventHandlers();
        }

        void SetEventHandlers()
        {
            lvEmployeeList.MouseUp += lvEmployeeList_MouseUp;
            lvEmployeeList.MouseDoubleClick += lvEmployeeList_MouseDoubleClick;
        }

        void lvEmployeeList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            viewModel.EditCommand.Execute(null);
        }

        void lvEmployeeList_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var selectedEmployee = lvEmployeeList.SelectedItem as Employee;
            if (selectedEmployee == null)
                return;

            viewModel.EditCommand.EmployeeId = selectedEmployee.Id;
            viewModel.DeleteCommand.EmployeeId = selectedEmployee.Id;
        }

        void UnsetEventHandlers()
        {
            lvEmployeeList.MouseUp += lvEmployeeList_MouseUp;
            lvEmployeeList.MouseDoubleClick += lvEmployeeList_MouseDoubleClick;
        }

        public override void Dispose()
        {
            UnsetEventHandlers();
            base.Dispose();
        }
    }

    public interface IEmployeeListView
    {
        
    }
}
