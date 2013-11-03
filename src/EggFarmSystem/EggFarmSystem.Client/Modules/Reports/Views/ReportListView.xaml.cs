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
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Core.Views;
using EggFarmSystem.Client.Modules.Reports.ViewModels;

namespace EggFarmSystem.Client.Modules.Reports.Views
{
    /// <summary>
    /// Interaction logic for ReportView.xaml
    /// </summary>
    public partial class ReportListView : UserControlBase, IReportListView
    {
        private ReportListViewModel model;
        private readonly IMessageBroker broker;

        public ReportListView(ReportListViewModel model, IMessageBroker broker)
        {
            InitializeComponent();
            this.DataContext = model;
            this.model = model;
            this.broker = broker;

            SetEventHandlers();
        }

        void SetEventHandlers()
        {
            lvReportList.MouseDoubleClick += lvReportList_MouseDoubleClick;
        }

        void lvReportList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var report = lvReportList.SelectedItem as ReportInfo;
            if (report == null || report.ViewType == null)
                return;

            broker.Publish(CommonMessages.ChangeMainView, report.ViewType);
        }

        void UnsetEventHandlers()
        {
            lvReportList.MouseDoubleClick -= lvReportList_MouseDoubleClick;
        }

        public override void Dispose()
        {
            UnsetEventHandlers();
            model.Dispose();
            base.Dispose();
        }
    }

    public interface IReportListView
    {
        
    }
}
