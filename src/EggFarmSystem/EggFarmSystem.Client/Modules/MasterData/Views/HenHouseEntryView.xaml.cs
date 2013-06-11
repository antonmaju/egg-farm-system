using System.Windows.Controls;
using EggFarmSystem.Client.Core.Views;
using EggFarmSystem.Client.Modules.MasterData.ViewModels;

namespace EggFarmSystem.Client.Modules.MasterData.Views
{
    /// <summary>
    /// Interaction logic for HenHouseForm.xaml
    /// </summary>
    public partial class HenHouseEntryView : UserControlBase, IHenHouseEntryView
    {
        public HenHouseEntryView(HouseEntryViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            this.NavigationCommands = viewModel.NavigationCommands;
        }
    }

    public interface IHenHouseEntryView
    {
        
    }
}
