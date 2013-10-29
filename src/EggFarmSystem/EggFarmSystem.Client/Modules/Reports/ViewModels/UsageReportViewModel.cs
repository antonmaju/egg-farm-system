using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Resources;
using EggFarmSystem.Services;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;

namespace EggFarmSystem.Client.Modules.Reports.ViewModels
{
    public class UsageReportViewModel : ViewModelBase
    {
        private DateTime startDate=DateTime.Today;
        private DateTime endDate=DateTime.Today;
        private Document document;

        public DelegateCommand ViewCommand;
        private readonly IReportingService service;

        public UsageReportViewModel(IReportingService service)
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
            NavigationCommands = new List<CommandBase>(){ViewCommand};
        }

        bool CanView(object param)
        {
            return EndDate <= StartDate;
        }

        void ViewReport(object param)
        {
            var document = new Document();
            document.UseCmykColor = true;
            var section = document.AddSection();
            section.PageSetup.TopMargin = Unit.FromCentimeter(4);

            var header = section.Headers.Primary;
            
            var paragraph = header.AddParagraph();

            //paragraph.Format.Font.Color = Color.FromCmyk(100, 30, 20, 50);
            paragraph.AddFormattedText("Usage Report", TextFormat.Bold);
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.Format.Font.Size = Unit.FromPoint(12);
            paragraph.Format.SpaceAfter = Unit.FromCentimeter(0.7);

            paragraph = header.AddParagraph();
            paragraph.AddFormattedText(string.Format("{0} {1} {2} {3}", LanguageData.General_From, StartDate.ToString("d MMMM yyyy"), LanguageData.General_To,
                                                     EndDate.ToString("d MMMM yyyy")), TextFormat.Bold);
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.Format.SpaceAfter = Unit.FromCentimeter(0);
            paragraph.Format.SpaceBefore = Unit.FromCentimeter(0);
            section.AddParagraph();

            //Table color etc
            var TableBorder = new Color(81, 125, 192);
            var TableBlue = new Color(235, 240, 249);
            var TableGray = new Color(242, 242, 242);

            // Create the item table
            var table1 = section.AddTable();
            table1.Style = "Table";
            table1.Borders.Color = TableBorder;
            table1.Borders.Width = 0.25;
            table1.Borders.Left.Width = 0.5;
            table1.Borders.Right.Width = 0.5;
            table1.Rows.LeftIndent = 0;

            //Set Column
            var column = table1.AddColumn();
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table1.AddColumn();
            column.Format.Alignment = ParagraphAlignment.Right;

            column = table1.AddColumn();
            column.Format.Alignment = ParagraphAlignment.Right;

            //Set Header
            var row = table1.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Shading.Color = TableBlue;
            row.Cells[0].AddParagraph("No");
            //row.Cells[0].Format.Font.Bold = false;
            row.Cells[0].Format.Alignment = ParagraphAlignment.Center;
            //row.Cells[0].VerticalAlignment = VerticalAlignment.Bottom;
            //row.Cells[0].MergeDown = 1;
            row.Cells[1].AddParagraph("Tanggal");
            //row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
            //row.Cells[1].MergeRight = 3;            
            row.Cells[2].AddParagraph("Jumlah");
            row.Cells[2].Format.Alignment = ParagraphAlignment.Center;
            //row.Cells[2].VerticalAlignment = VerticalAlignment.Bottom;
            //row.Cells[2].MergeDown = 1;

            for (int i = 1; i <= 300; i++)
            {
                var row1 = table1.AddRow();
                row1.Cells[0].AddParagraph(i.ToString());
                row1.Cells[0].Format.Alignment = ParagraphAlignment.Left;

                row1.Cells[1].AddParagraph("tanggal " + i.ToString());
                row1.Cells[1].Format.Alignment = ParagraphAlignment.Left;

                //row1.Cells[2].AddParagraph("jumlah " + i.ToString());
                row1.Cells[2].AddParagraph(i.ToString());
                row1.Cells[2].Format.Alignment = ParagraphAlignment.Right;
            }

            var row2 = table1.AddRow();

            row2.Format.Alignment = ParagraphAlignment.Center;
            row2.Format.Font.Bold = true;
            row2.Shading.Color = TableBlue;
            row2.Cells[0].AddParagraph("Total");
            row2.Cells[0].MergeRight = 1;
            row2.Cells[0].Format.Alignment = ParagraphAlignment.Center;
            row2.Cells[2].AddParagraph("TOTALNYA");
            row2.Cells[2].Format.Alignment = ParagraphAlignment.Right;


            //var summary = service.GetEmployeeCostSummary(StartDate, EndDate);

            //var document = new Document();
            //document.UseCmykColor = true;
            //var section = document.AddSection();

            //var header = section.Headers.Primary;
            //header.Format.SpaceAfter = Unit.FromCentimeter(2);
            //var paragraph = header.AddParagraph();
            //paragraph.AddFormattedText(LanguageData.EmployeeCostReport_Title, TextFormat.Bold);
            //paragraph.Format.Alignment = ParagraphAlignment.Center;
            //paragraph.AddLineBreak();
            //paragraph.AddLineBreak();
            //paragraph.AddFormattedText(string.Format("{0} {1} {2} {3}", LanguageData.General_From, StartDate.ToString("d MMMM yyyy"), LanguageData.General_To,
            //                                         EndDate.ToString("d MMMM yyyy")));
            //paragraph.Format.Alignment = ParagraphAlignment.Center;
            //paragraph.Format.SpaceAfter = Unit.FromCentimeter(2);
            

            //var table = new Table();
            //table.Borders.Width = 0.75;
            
            //var column = table.AddColumn(Unit.FromCentimeter(5));
            //column = table.AddColumn(Unit.FromCentimeter(2));
            //column.Format.Alignment = ParagraphAlignment.Right;
            //column = table.AddColumn(Unit.FromCentimeter(4));
            //column.Format.Alignment = ParagraphAlignment.Right;

            //Row row = table.AddRow();
            //row.TopPadding = Unit.FromCentimeter(0.4);
            //row.BottomPadding = Unit.FromCentimeter(0.4);
            //var cell = row.Cells[0];
            //cell.AddParagraph(LanguageData.EmployeeCostReport_NameField);
            //cell.Format.Alignment = ParagraphAlignment.Center; 
            //cell = row.Cells[1];
            //cell.AddParagraph(LanguageData.EmployeeCostReport_DaysField);
            //cell.Format.Alignment = ParagraphAlignment.Center; 
            //cell = row.Cells[2];
            //cell.AddParagraph(LanguageData.EmployeeCostReport_TotalSalaryField);
            //cell.Format.Alignment = ParagraphAlignment.Center; 

            //foreach (var summaryItem in summary)
            //{
            //    row = table.AddRow();
            //    row.TopPadding = Unit.FromCentimeter(0.2);
            //    row.BottomPadding = Unit.FromCentimeter(0.2);
            //    cell = row.Cells[0];
            //    cell.AddParagraph(summaryItem.Name);
            //    cell = row.Cells[1];
            //    cell.AddParagraph(summaryItem.Days.ToString());
            //    cell = row.Cells[2];
            //    cell.AddParagraph(summaryItem.TotalSalary.ToString());
            //}

            //document.LastSection.Add(table);

            //var row2 = table1.AddRow();
            
            //row2.Format.Alignment = ParagraphAlignment.Center;
            //row2.Format.Font.Bold = true;
            //row2.Shading.Color = TableBlue;
            //row2.Cells[0].AddParagraph("Total");
            //row2.Cells[0].MergeRight = 1;  
            //row2.Cells[0].Format.Alignment = ParagraphAlignment.Center;
            //row2.Cells[2].AddParagraph("TOTALNYA");
            //row2.Cells[2].Format.Alignment = ParagraphAlignment.Right;
            
            Document = document;
        }

        #endregion

       

    }
}
