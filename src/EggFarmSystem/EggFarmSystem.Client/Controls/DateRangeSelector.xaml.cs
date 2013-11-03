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
using EggFarmSystem.Resources;

namespace EggFarmSystem.Client.Controls
{
    /// <summary>
    /// Interaction logic for DateRange.xaml
    /// </summary>
    public partial class DateRangeSelector : UserControl
    {
        public DateRangeSelector()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty FromProperty = DependencyProperty.Register("From",
                                                                                      typeof(DateTime),
                                                                                      typeof(DateRangeSelector),new PropertyMetadata(DateTime.Today));
 
        public DateTime From
        {
            get { return (DateTime) GetValue(FromProperty); }
            set { SetValue(FromProperty, value);}
        }

        public static readonly DependencyProperty ToProperty = DependencyProperty.Register("To",
                                                                                      typeof(DateTime),
                                                                                      typeof(DateRangeSelector), new PropertyMetadata(DateTime.Today));

        public DateTime To
        {
            get { return (DateTime)GetValue(ToProperty); }
            set { SetValue(ToProperty, value); }
        }

        public string FromText { get { return LanguageData.General_From; } }

        public string ToText { get { return LanguageData.General_To; } }
    }
}
