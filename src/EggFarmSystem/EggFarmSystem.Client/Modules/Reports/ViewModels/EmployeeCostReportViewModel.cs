using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Models.Reporting;
using EggFarmSystem.Resources;
using EggFarmSystem.Services;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;

namespace EggFarmSystem.Client.Modules.Reports.ViewModels
{
    public class EmployeeCostReportViewModel : ViewModelBase
    {
        private DateTime startDate=DateTime.Today;
        private DateTime endDate=DateTime.Today;
        private Document document;

        public DelegateCommand ViewCommand, ExportCommand;
        private readonly IReportingService service;

        public EmployeeCostReportViewModel(IReportingService service)
        {
            InitializeCommands();

            this.service = service;
        }

        #region properties

        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; OnPropertyChanged("StartDate"); }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; OnPropertyChanged("EndDate"); }
        }

        public Document Document
        {
            get { return document; }
            set
            {
                document = value;
                OnPropertyChanged("Document");
            }
        }

        public IList<CommandBase> NavigationCommands { get; private set; }

        #endregion

        #region commands

        private void InitializeCommands()
        {
            ViewCommand = new DelegateCommand(ViewReport, CanView){Text = ()=> LanguageData.General_View};
            ExportCommand = new DelegateCommand(Export, CanExport){Text=()=>LanguageData.General_Export};
            NavigationCommands = new List<CommandBase>(){ViewCommand};
        }

        bool CanExport(object param)
        {
            return Document != null;
        }

        void Export(object param)
        {
            
        }

        bool CanView(object param)
        {
            return EndDate >= StartDate;
        }

        void ViewReport(object param)
        {
            IList<Models.Reporting.EmployeeCostSummary> summary = service.GetEmployeeCostSummary(StartDate, EndDate);

            //IList<Models.Reporting.EmployeeCostSummary> summary = new List<EmployeeCostSummary>();
            //for (int i = 0; i < 100; i++)
            //{
            //    summary.Add(new EmployeeCostSummary
            //        {
            //            Days = 50,
            //            Id = Guid.NewGuid(),
            //            Name ="Employee #"+i,
            //            TotalSalary = 100000
            //        });
            //}

            var document = new Document();
            document.UseCmykColor = true;
            var section = document.AddSection();
            section.PageSetup.TopMargin = Unit.FromCentimeter(4);

            var header = section.Headers.Primary;
            

            var paragraph = header.AddParagraph();
            paragraph.AddFormattedText(LanguageData.EmployeeCostReport_Title, TextFormat.Bold);
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.Format.Font.Size = Unit.FromPoint(12);
            paragraph.Format.SpaceAfter = Unit.FromCentimeter(0.7);

            paragraph = header.AddParagraph();
            paragraph.AddFormattedText(string.Format("{0} {1} {2} {3}", LanguageData.General_From, StartDate.ToString("d MMMM yyyy"), LanguageData.General_To,
                                                     EndDate.ToString("d MMMM yyyy")),TextFormat.Bold);
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.Format.SpaceAfter = Unit.FromCentimeter(0);
            paragraph.Format.SpaceBefore = Unit.FromCentimeter(0);

            var table = section.AddTable();
            table.KeepTogether = false;
            table.Borders.Width = 0.75;

            var column = table.AddColumn(Unit.FromCentimeter(8));
            column = table.AddColumn(Unit.FromCentimeter(3));
            column.Format.Alignment = ParagraphAlignment.Right;
            column = table.AddColumn(Unit.FromCentimeter(5));
            column.Format.Alignment = ParagraphAlignment.Right;

            Row row = table.AddRow();
            row.TopPadding = Unit.FromCentimeter(0.4);
            row.BottomPadding = Unit.FromCentimeter(0.4);
            row.Format.Alignment = ParagraphAlignment.Center; 

            var cell = row.Cells[0];
            cell.AddParagraph(LanguageData.EmployeeCostReport_NameField);
            cell.Format.Alignment = ParagraphAlignment.Center; 
            cell = row.Cells[1];
            cell.AddParagraph(LanguageData.EmployeeCostReport_DaysField);
            cell.Format.Alignment = ParagraphAlignment.Center; 
            cell = row.Cells[2];
            cell.AddParagraph(LanguageData.EmployeeCostReport_TotalSalaryField);
            cell.Format.Alignment = ParagraphAlignment.Center; 

            foreach (var summaryItem in summary)
            {
                row = table.AddRow();
                row.TopPadding = Unit.FromCentimeter(0.2);
                row.BottomPadding = Unit.FromCentimeter(0.2);
                row.Format.Alignment = ParagraphAlignment.Center;
                cell = row.Cells[0];
                cell.Format.Alignment = ParagraphAlignment.Left;
                cell.AddParagraph(summaryItem.Name);
                cell = row.Cells[1];
                cell.Format.Alignment = ParagraphAlignment.Right;
                cell.AddParagraph(summaryItem.Days.ToString());
                cell = row.Cells[2];
                cell.Format.Alignment = ParagraphAlignment.Right;
                cell.AddParagraph(summaryItem.TotalSalary.ToString());
            }

            Document = document;
        }

        #endregion

       

    }
}
