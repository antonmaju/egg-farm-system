using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using EggFarmSystem.Resources;
using MigraDoc.Rendering;
using MigraDoc.Rendering.Windows;
using MigraDoc.RtfRendering;

namespace EggFarmSystem.Client.Resources
{
    public partial class DocumentViewerResourceDictionary
    {
        private const string ReportDir = "Reports";

        private const string DefaultReportName = "Report";

        public DocumentViewerResourceDictionary()
        {
            InitializeComponent();
        }

        private void ExportPdf_Click(object sender, RoutedEventArgs args)
        {
            var viewer = (DocumentViewer) (sender as Button).TemplatedParent;

            if (viewer.Document == null)
                return;

            var preview = FindParent<DocumentPreview>(viewer);

            if (preview == null)
                return;

            var renderer = new PdfDocumentRenderer();
            renderer.DocumentRenderer = preview.Renderer;
            renderer.Document = preview.Document;
            renderer.RenderDocument();
            preview.Document.BindToRenderer(null);

            try
            {
                string reportFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ReportDir);

                if (! Directory.Exists(reportFolder))
                    Directory.CreateDirectory(reportFolder);

                string path = Path.Combine(reportFolder, (preview.Tag != null ? preview.Tag.ToString() : DefaultReportName) + ".pdf");  
                renderer.Save(path);

                Process.Start(path);
            
            }
            catch(Exception ex)
            {
                MessageBox.Show(LanguageData.Reports_PermissionIssue);
            }
        }

        private void ExportRtf_Click(object sender, RoutedEventArgs args)
        {
            var viewer = (DocumentViewer)(sender as Button).TemplatedParent;

            if (viewer.Document == null)
                return;

            var preview = FindParent<DocumentPreview>(viewer);

            if (preview == null)
                return;

            var renderer = new RtfDocumentRenderer();
 
            try
            {
                string reportFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ReportDir);

                if (!Directory.Exists(reportFolder))
                    Directory.CreateDirectory(reportFolder);

                string path = Path.Combine(reportFolder, (preview.Tag != null ? preview.Tag.ToString() : DefaultReportName) + ".rtf");
                
                renderer.Render(preview.Document,path, reportFolder);

                Process.Start(path);

            }
            catch (Exception ex)
            {
                MessageBox.Show(LanguageData.Reports_PermissionIssue);
            }

        }

        private static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {

            var parent = VisualTreeHelper.GetParent(child);

            if (parent == null) return null;

            var parentEl = parent as T;

            return parentEl ?? FindParent<T>(parent);
        }
    }
}
