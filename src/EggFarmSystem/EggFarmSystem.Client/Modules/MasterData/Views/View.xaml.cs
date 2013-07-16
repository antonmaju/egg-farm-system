using System.Windows;
using System.Windows.Controls;
using EggFarmSystem.Client.Core.Views;
using EggFarmSystem.Client.Modules.MasterData.ViewModels;

namespace EggFarmSystem.Client.Modules.MasterData.Views
{
    /// <summary>
    /// Interaction logic for View.xaml
    /// </summary>
    public partial class View : UserControlBase, IMasterDataView
    {
        private MasterDataViewModel viewModel;

        public View(MasterDataViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.DataContext = viewModel;
            this.Loaded += View_Loaded;
        }

        void View_Loaded(object sender, RoutedEventArgs e)
        {
            (this.DataContext as MasterDataViewModel).InitializeContent();
        }

        public override void Dispose()
        {
            this.Loaded -= View_Loaded;
            this.viewModel.Dispose();
            base.Dispose();
        }
    }

    public interface IMasterDataView
    {
        
    }
}
