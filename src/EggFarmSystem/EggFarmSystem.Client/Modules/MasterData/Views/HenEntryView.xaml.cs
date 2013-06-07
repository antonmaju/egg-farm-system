using System.Windows.Controls;
using EggFarmSystem.Client.Core.Views;
using EggFarmSystem.Client.Modules.MasterData.ViewModels;

namespace EggFarmSystem.Client.Modules.MasterData.Views
{
    /// <summary>
    /// Interaction logic for AyamForm.xaml
    /// </summary>
    public partial class HenEntryView : UserControlBase, IHenEntryView
    {
        public HenEntryView(HenEntryViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            this.NavigationCommands = viewModel.NavigationCommands;
        }
    }

    public interface IHenEntryView
    {
        
    }
}
