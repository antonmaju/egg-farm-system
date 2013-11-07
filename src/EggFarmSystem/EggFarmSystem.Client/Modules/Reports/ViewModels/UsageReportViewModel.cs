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

        public string Title { get { return LanguageData.UsageReport_Title; } }

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
            IList<UsageSummary> reportList = service.GetUsageSummary(StartDate, EndDate);

            var document = new Document();
            document.UseCmykColor = true;

            foreach (var report in reportList)
            {
                var section = document.AddSection();
                section.PageSetup.TopMargin = Unit.FromCentimeter(2);
                //section.PageSetup.Orientation = Orientation.Landscape;

                var paragraph = section.AddParagraph();
                paragraph.Format.Alignment = ParagraphAlignment.Center;
                paragraph.Format.Font.Size = Unit.FromPoint(12);
                paragraph.AddFormattedText(LanguageData.UsageReport_Title, TextFormat.Bold);
                paragraph.AddLineBreak();

                paragraph = section.AddParagraph();
                paragraph.Format.Alignment = ParagraphAlignment.Center;
                paragraph.AddFormattedText(report.Date.ToString("d MMMM yyyy"));
                paragraph.Format.Alignment = ParagraphAlignment.Center;
                paragraph.Format.SpaceAfter = Unit.FromCentimeter(1);
                paragraph.Format.SpaceBefore = Unit.FromCentimeter(0);

                var table = section.AddTable();
                table.KeepTogether = false;
                table.Borders.Width = 0.75;

                var column = table.AddColumn(Unit.FromCentimeter(4));
                column.Format.Alignment = ParagraphAlignment.Left;
                column = table.AddColumn(Unit.FromCentimeter(4));
                column.Format.Alignment = ParagraphAlignment.Right;
                column = table.AddColumn(Unit.FromCentimeter(2));
                column.Format.Alignment = ParagraphAlignment.Right;
                column = table.AddColumn(Unit.FromCentimeter(3));
                column.Format.Alignment = ParagraphAlignment.Right;
                column = table.AddColumn(Unit.FromCentimeter(3));
                column.Format.Alignment = ParagraphAlignment.Right;

                var row = table.AddRow();
                row.TopPadding = Unit.FromCentimeter(0.4);
                row.BottomPadding = Unit.FromCentimeter(0.4);
                row.Format.Alignment = ParagraphAlignment.Center;

                var cell = row.Cells[0];
                cell.AddParagraph(LanguageData.Usage_HouseField);
                cell.Format.Alignment = ParagraphAlignment.Center;
                cell = row.Cells[1];
                cell.AddParagraph(LanguageData.Usage_ConsumableField);
                cell.Format.Alignment = ParagraphAlignment.Center;
                cell = row.Cells[2];
                cell.AddParagraph(LanguageData.Usage_CountField);
                cell.Format.Alignment = ParagraphAlignment.Center;
                cell = row.Cells[3];
                cell.AddParagraph(LanguageData.Usage_UnitPriceField);
                cell.Format.Alignment = ParagraphAlignment.Center;
                cell = row.Cells[4];
                cell.AddParagraph(LanguageData.Usage_SubTotalField);
                cell.Format.Alignment = ParagraphAlignment.Center;

                foreach (var detail in report.Details)
                {
                    row = table.AddRow();
                    row.TopPadding = Unit.FromCentimeter(0.2);
                    row.BottomPadding = Unit.FromCentimeter(0.2);

                    cell = row.Cells[0];
                    cell.AddParagraph(detail.House.Name);
                    cell.Format.Alignment = ParagraphAlignment.Left;
                    cell = row.Cells[1];
                    cell.AddParagraph(detail.Consumable.Name);
                    cell.Format.Alignment = ParagraphAlignment.Left;
                    cell = row.Cells[2];
                    cell.AddParagraph(detail.Count.ToString());
                    cell.Format.Alignment = ParagraphAlignment.Right;
                    cell = row.Cells[3];
                    cell.AddParagraph(detail.UnitPrice.ToString("0,00"));
                    cell.Format.Alignment = ParagraphAlignment.Right;
                    cell = row.Cells[4];
                    cell.AddParagraph(detail.SubTotal.ToString("#,##"));
                    cell.Format.Alignment = ParagraphAlignment.Right;
                }
                row = table.AddRow();
                row.TopPadding = Unit.FromCentimeter(0.2);
                row.BottomPadding = Unit.FromCentimeter(0.2);

                cell = row.Cells[0];                
                cell.AddParagraph(LanguageData.Usage_TotalField);
                cell.Format.Alignment = ParagraphAlignment.Center;
                cell.MergeRight = 3;
                cell.Format.Font.Bold = true;
                cell = row.Cells[4];
                cell.AddParagraph(report.Total.ToString("#,##"));
                cell.Format.Alignment = ParagraphAlignment.Right;
                cell.Format.Font.Bold = true;
                
            }

            Document = document;
        }
        #endregion

       

    }
}
