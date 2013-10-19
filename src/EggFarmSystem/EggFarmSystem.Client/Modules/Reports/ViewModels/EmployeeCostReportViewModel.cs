using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Resources;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;

namespace EggFarmSystem.Client.Modules.Reports.ViewModels
{
    public class EmployeeCostReportViewModel : ViewModelBase
    {
        private DateTime startDate=DateTime.Today;
        private DateTime endDate=DateTime.Today;
        private Document document;

        public DelegateCommand ViewCommand;

        public EmployeeCostReportViewModel()
        {
            InitializeCommands();

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
            var paragraph = section.AddParagraph();
            
            paragraph.Format.Font.Color = Color.FromCmyk(100, 30, 20, 50);
            paragraph.AddFormattedText("Employee Cost Report", TextFormat.Bold);
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

            for (int i = 1; i <= 15; i++)
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
            
            Document = document;
        }

        #endregion

       

    }
}
