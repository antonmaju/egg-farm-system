using System.Windows.Controls;
using EggFarmSystem.Client.Core.Views;
using EggFarmSystem.Client.Modules.MasterData.ViewModels;

namespace EggFarmSystem.Client.Modules.MasterData.Views
{
    /// <summary>
    /// Interaction logic for EmployeeForm.xaml
    /// </summary>
    public partial class EmployeeEntryView : UserControlBase, IEmployeeEntryView
    {
        public EmployeeEntryView(EmployeeEntryViewModel viewModel)
        {
            InitializeComponent();

            this.DataContext = viewModel;
            this.NavigationCommands = viewModel.NavigationCommands;
        }
    }

    public interface IEmployeeEntryView
    {
        
    }
}
