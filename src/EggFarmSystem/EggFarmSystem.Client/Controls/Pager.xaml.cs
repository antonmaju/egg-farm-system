using System.Collections.ObjectModel;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Models;
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

namespace EggFarmSystem.Client.Controls
{
    /// <summary>
    /// Interaction logic for Pager.xaml
    /// </summary>
    public partial class Pager : UserControl
    {
        private DelegateCommand firstCommand;
        private DelegateCommand prevCommand;
        private DelegateCommand nextCommand;
        private DelegateCommand lastCommand;

        public Pager()
        {
            InitializeComponent();
            SetCommandBindings();
        }

        public static readonly DependencyProperty PagesProperty = DependencyProperty.Register("Pages",
                                                                                      typeof (ObservableCollection<int>),
                                                                                      typeof (Pager));

        public ObservableCollection<int> Pages
        {
            get { return GetValue(PagesProperty) as ObservableCollection<int>; }
            set { SetValue(PagesProperty, value); }
        }

        public static readonly DependencyProperty PageIndexProperty = DependencyProperty.Register("PageIndex",
                                                                                                     typeof (int),
                                                                                                     typeof(Pager), new PropertyMetadata(0, OnPageIndexChanged));

        public int PageIndex
        {
            get { return (int) GetValue(PageIndexProperty); }
            set { SetValue(PageIndexProperty, value); }
        }

        public static readonly DependencyProperty TotalPageProperty = DependencyProperty.Register("TotalPage",
                                                                                                     typeof(int),
                                                                                                     typeof(Pager),
                                                                                                     new PropertyMetadata(0,OnTotalPageChanged));

        public int TotalPage
        {
            get { return (int)GetValue(TotalPageProperty); }
            set { SetValue(TotalPageProperty, value); }
        }

        private void SetCommandBindings()
        {
             firstCommand = new DelegateCommand(param => TriggerPageIndexChanged(1),
                 param => PageIndex > 1);

             prevCommand = new DelegateCommand(param => TriggerPageIndexChanged(PageIndex - 1),
                 param => PageIndex > 1);

             nextCommand = new DelegateCommand(param => TriggerPageIndexChanged(PageIndex + 1),
                 param => PageIndex < TotalPage);

             lastCommand = new DelegateCommand(param => TriggerPageIndexChanged(TotalPage),
                 param => PageIndex < TotalPage);

            btnFirst.Command = firstCommand;
            btnPrev.Command = prevCommand;
            btnNext.Command = nextCommand;
            btnLast.Command = lastCommand;

        }

        private static void OnPageIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pager = d as Pager;

            if (pager == null) return;

            if (pager.PageIndexChanged != null)
            {
                pager.PageIndexChanged(pager, new PagerEventArgs { PageIndex = pager.PageIndex });
            }
        }
        

        private static void OnTotalPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pager = d as Pager;

            if (pager == null)
                return;

            pager.Pages = new ObservableCollection<int>(Enumerable.Range(1,pager.TotalPage));

            if(pager.TotalPage <= 1)
                pager.Visibility = Visibility.Hidden; 

        }


        private void TriggerPageIndexChanged(int pageIndex)
        {
            PageIndex = pageIndex;
        }

        public event PagerHandler PageIndexChanged;
    }

    public delegate void PagerHandler(object sender, PagerEventArgs args);

    public class PagerEventArgs : EventArgs
    {
        public int PageIndex { get; set; }
    }
}
