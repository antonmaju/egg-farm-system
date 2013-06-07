using System.Windows.Controls;
using EggFarmSystem.Client.Core.Views;

namespace EggFarmSystem.Client.Modules.MasterData.Views
{
    /// <summary>
    /// Interaction logic for HenHouseForm.xaml
    /// </summary>
    public partial class HenHouseEntryView : UserControlBase, IHenHouseEntryView
    {
        public HenHouseEntryView()
        {
            InitializeComponent();
        }
    }

    public interface IHenHouseEntryView
    {
        
    }
}
