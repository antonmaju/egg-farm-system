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
using EggFarmSystem.Client.Modules.EggProduction.ViewModels;

namespace EggFarmSystem.Client.Modules.EggProduction.Views
{
    /// <summary>
    /// Interaction logic for EggProductionListView.xaml
    /// </summary>
    public partial class EggProductionListView : UserControlBase, IEggProductionListView
    {
        private readonly EggProductionListViewModel model;

        public EggProductionListView(EggProductionListViewModel model)
        {
            InitializeComponent();

            this.model = model;
            this.DataContext = model;
            this.NavigationCommands = model.NavigationCommands;

            SetEventHandlers();
        }

        void SetEventHandlers()
        {
            lvProductionList.MouseUp += lvProductionList_MouseUp;
            lvProductionList.MouseDoubleClick += lvProductionList_MouseDoubleClick;
            pager.PageIndexChanged += pager_PageIndexChanged;
        }

        void pager_PageIndexChanged(object sender, Controls.PagerEventArgs args)
        {
            model.PageIndex = args.PageIndex;
        }

        void lvProductionList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            model.EditCommand.Execute(null);
        }

        void lvProductionList_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var production = lvProductionList.SelectedItem as Models.EggProduction;
            if (production == null)
                return;

            model.EditCommand.EntityId = production.Id;
            model.DeleteCommand.EntityId = production.Id;
        }

        void UnsetEventHandlers()
        {
            lvProductionList.MouseUp -= lvProductionList_MouseUp;
            lvProductionList.MouseDoubleClick -= lvProductionList_MouseDoubleClick;
            pager.PageIndexChanged -= pager_PageIndexChanged;
        }

        public override void Dispose()
        {
            UnsetEventHandlers();
            model.Dispose();

            base.Dispose();
        }
    }

    public interface IEggProductionListView
    {

    }
}
