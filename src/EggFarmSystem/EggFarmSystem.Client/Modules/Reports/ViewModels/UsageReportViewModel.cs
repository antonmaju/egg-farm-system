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
    public class UsageReportViewModel : ViewModelBase
    {
        private DateTime startDate=DateTime.Today;
        private DateTime endDate=DateTime.Today;
        private Document document;

        public DelegateCommand ViewCommand, ExportCommand;
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
            return EndDate >= StartDate;
        }

        void ViewReport(object param)
        {
            //EndDate = "12/12/2005";
            IList<Models.Reporting.UsageSummary> summary = service.GetUsageSummary(StartDate, EndDate);
            
            var document = new Document();
            document.UseCmykColor = true;
            var section = document.AddSection();
            section.PageSetup.TopMargin = Unit.FromCentimeter(4);
            section.PageSetup.Orientation = Orientation.Landscape;
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
            Table table= new Table();
            Row row;
            Cell cell;
            Column column;
            long _subtotal = 0;

            var LastSummaryId = new Guid();
            var LastUsageDate = "0";
            foreach (var summaryItem in summary)
            {
                
                if (summaryItem.UsageDate.ToString() != LastUsageDate)
                {
                    //LastSummaryId = summaryItem.Id;
                    if (LastUsageDate != "0")
                    {
                        row = table.AddRow();
                        row.Format.Font.Bold = true;
                        cell = row.Cells[0];
                        cell.AddParagraph("Total");
                        cell.MergeRight = 3;
                        cell = row.Cells[4];
                        cell.AddParagraph(_subtotal.ToString());
                        _subtotal = 0;
                        section.AddPageBreak();
                    }
                    table = section.AddTable();
                    table.Style = "Table";
                    table.Borders.Color = TableBorder;
                    table.KeepTogether = false;
                    table.Borders.Width = 0.75;

                    //Set Column
                    column = table.AddColumn(Unit.FromCentimeter(6));
                    column = table.AddColumn(Unit.FromCentimeter(6));
                    column.Format.Alignment = ParagraphAlignment.Right;
                    column = table.AddColumn(Unit.FromCentimeter(4));
                    column.Format.Alignment = ParagraphAlignment.Right;
                    column = table.AddColumn(Unit.FromCentimeter(4));
                    column.Format.Alignment = ParagraphAlignment.Right;
                    column = table.AddColumn(Unit.FromCentimeter(4));
                    column.Format.Alignment = ParagraphAlignment.Right;

                    //Set Header
                    row = table.AddRow();
                    row.Format.Font.Bold = true;
                    row.Shading.Color = TableBlue;
                    cell = row.Cells[0];
                    cell.AddParagraph(summaryItem.UsageDate.ToShortDateString());
                    cell.MergeRight = 4;

                    row = table.AddRow();
                    row.HeadingFormat = true;
                    row.Format.Font.Bold = true;
                    row.Shading.Color = TableBlue;
                    cell = row.Cells[0];
                    cell.AddParagraph("Kandang");
                    row.Cells[0].Format.Alignment = ParagraphAlignment.Center;
                    cell = row.Cells[1];
                    cell.AddParagraph("Nama Pakan/OVK");
                    cell.Format.Alignment = ParagraphAlignment.Center;
                    cell = row.Cells[2];
                    cell.AddParagraph("Jumlah");
                    cell.Format.Alignment = ParagraphAlignment.Center;
                    cell = row.Cells[3];
                    cell.AddParagraph("Harga Per Unit");
                    cell.Format.Alignment = ParagraphAlignment.Center;
                    cell = row.Cells[4];
                    cell.AddParagraph("Subtotal");
                    cell.Format.Alignment = ParagraphAlignment.Center;

                    LastUsageDate = summaryItem.UsageDate.ToString();

                    
                    //cell.MergeDown = 1;
                }
                _subtotal = _subtotal+summaryItem.SubTotal;
                
                row = table.AddRow();
                row.TopPadding = Unit.FromCentimeter(0.2);
                row.BottomPadding = Unit.FromCentimeter(0.2);
                row.Format.Alignment = ParagraphAlignment.Center;
                cell = row.Cells[0];
                cell.Format.Alignment = ParagraphAlignment.Left;
                cell.AddParagraph(summaryItem.HenHouseName);
                cell = row.Cells[1];
                cell.Format.Alignment = ParagraphAlignment.Left;
                cell.AddParagraph(summaryItem.Name);
                cell = row.Cells[2];
                cell.Format.Alignment = ParagraphAlignment.Right;
                cell.AddParagraph(summaryItem.Count.ToString());
                cell = row.Cells[3];
                cell.Format.Alignment = ParagraphAlignment.Right;
                cell.AddParagraph(summaryItem.UnitPrice.ToString());
                cell = row.Cells[4];
                cell.Format.Alignment = ParagraphAlignment.Right;
                cell.AddParagraph(summaryItem.SubTotal.ToString());

                if(object.ReferenceEquals(summary.Last(), summaryItem))
                {
                    row = table.AddRow();
                    row.Format.Font.Bold = true;
                    cell = row.Cells[0];
                    cell.AddParagraph("Total");
                    cell.MergeRight = 3;
                    cell = row.Cells[4];
                    cell.AddParagraph(_subtotal.ToString());
                    _subtotal = 0;
                }

            }

            Document = document;
        }

        #endregion

       

    }
}
