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
using EggFarmSystem.Client.Modules.HenDepreciation.ViewModels;

namespace EggFarmSystem.Client.Modules.HenDepreciation.Views
{
    /// <summary>
    /// Interaction logic for HenDepreciationListView.xaml
    /// </summary>
    public partial class HenDepreciationListView : UserControlBase, IHenDepreciationListView
    {

        private readonly HenDepreciationListViewModel model;

        public HenDepreciationListView(HenDepreciationListViewModel model)
        {
            InitializeComponent();

            this.model = model;
            this.DataContext = model;
            this.NavigationCommands = model.NavigationCommands;

            SetEventHandlers();
        }

        void SetEventHandlers()
        {
            lvDepreciationList.MouseUp += lvDepreciationList_MouseUp;
            lvDepreciationList.MouseDoubleClick += lvDepreciationList_MouseDoubleClick;
        }

        void lvDepreciationList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            model.EditCommand.Execute(null);
        }

        void lvDepreciationList_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var depreciation = lvDepreciationList.SelectedItem as Models.HenDepreciation;
            if (depreciation == null)
                return;

            model.EditCommand.EntityId = depreciation.Id;
            model.DeleteCommand.EntityId = depreciation.Id;
        }

        void UnsetEventHandlers()
        {
            lvDepreciationList.MouseUp -= lvDepreciationList_MouseUp;
            lvDepreciationList.MouseDoubleClick -= lvDepreciationList_MouseDoubleClick;
        }

        public override void Dispose()
        {
            UnsetEventHandlers();
            model.Dispose();

            base.Dispose();
        }
    }

    public interface IHenDepreciationListView
    {

    }
}
