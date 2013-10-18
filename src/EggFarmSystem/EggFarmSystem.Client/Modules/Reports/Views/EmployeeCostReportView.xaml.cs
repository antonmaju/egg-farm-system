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
using EggFarmSystem.Client.Modules.Reports.ViewModels;
using MigraDoc.DocumentObjectModel.IO;

namespace EggFarmSystem.Client.Modules.Reports.Views
{
    /// <summary>
    /// Interaction logic for EmployeeCostReportView.xaml
    /// </summary>
    public partial class EmployeeCostReportView : UserControlBase, IEmployeeCostReportView
    {
        private EmployeeCostReportViewModel model;

        public EmployeeCostReportView(EmployeeCostReportViewModel model)
        {
            InitializeComponent();
            this.DataContext = model;
            this.NavigationCommands = model.NavigationCommands;
            this.model = model;
            SetEventHandlers();

        }

        private void SetEventHandlers()
        {
            model.PropertyChanged += model_PropertyChanged;
        }

        void model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Document")
            {
                if (model.Document != null)
                {
                    string ddl = DdlWriter.WriteToString(model.Document);
                    docViewer.Ddl = ddl;
                }
            }
        }

        private void UnsetEventHandlers()
        {
            model.PropertyChanged -= model_PropertyChanged;
        }

        public override void Dispose()
        {
            UnsetEventHandlers();
            model.Dispose();
            base.Dispose();
        }
    }

    public interface IEmployeeCostReportView
    {
        
    }
}
