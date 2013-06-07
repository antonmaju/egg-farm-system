using System.Windows.Controls;
using EggFarmSystem.Client.Core.Views;

namespace EggFarmSystem.Client.Modules.MasterData.Views
{
    /// <summary>
    /// Interaction logic for EmployeeForm.xaml
    /// </summary>
    public partial class EmployeeEntryView : UserControlBase, IEmployeeEntryView
    {
        public EmployeeEntryView()
        {
            InitializeComponent();
        }
    }

    public interface IEmployeeEntryView
    {
        
    }
}
