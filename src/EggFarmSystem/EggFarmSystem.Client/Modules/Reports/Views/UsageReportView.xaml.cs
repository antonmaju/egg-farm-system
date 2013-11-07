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
using PdfSharp.Pdf;

namespace EggFarmSystem.Client.Modules.Reports.Views
{
    /// <summary>
    /// Interaction logic for UsageReportView.xaml
    /// </summary>
    public partial class UsageReportView : UserControlBase, IUsageReportView
    {
        private UsageReportViewModel model;

        public UsageReportView(UsageReportViewModel model)
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
            const bool unicode = false;
            const PdfFontEmbedding embedding = PdfFontEmbedding.Always;
            if (e.PropertyName == "Document")
            {
                if (model.Document != null)
                {
                    docViewer.Ddl = DdlWriter.WriteToString(model.Document);
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

    public interface IUsageReportView
    {
        
    }
}
