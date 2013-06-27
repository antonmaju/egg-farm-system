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
        private IPagingInfo pagingInfo;
        private DelegateCommand firstCommand;
        private DelegateCommand prevCommand;
        private DelegateCommand nextCommand;
        private DelegateCommand lastCommand;
        private ObservableCollection<int> numbers;

        public Pager()
        {
            InitializeComponent();
            numbers = new ObservableCollection<int>();
            SetCommandBindings();
        }

        public IPagingInfo PagingSource
        {
            get { return pagingInfo; }
            set 
            { 
                ClearBindings();
                pagingInfo = value;
                this.DataContext = value;
                SetBindings();
            }
        }

        private void SetCommandBindings()
        {
             firstCommand = new DelegateCommand(param => TriggerPageIndexChanged(1),
                 param => pagingInfo != null);

             prevCommand = new DelegateCommand(param => TriggerPageIndexChanged(pagingInfo.PageIndex - 1),
                 param => pagingInfo != null && pagingInfo.PageIndex > 1);

             nextCommand = new DelegateCommand(param => TriggerPageIndexChanged(pagingInfo.PageIndex + 1),
                 param => pagingInfo != null && pagingInfo.PageIndex < pagingInfo.TotalPage);

             lastCommand = new DelegateCommand(param => TriggerPageIndexChanged(pagingInfo.TotalPage),
                 param => pagingInfo != null);

            btnFirst.Command = firstCommand;
            btnPrev.Command = prevCommand;
            btnNext.Command = nextCommand;
            btnLast.Command = lastCommand;

        }

        private void SetBindings()
        {
            var sourceBinding = new Binding();
            sourceBinding.Source = numbers;
            BindingOperations.SetBinding(cboPage, ComboBox.ItemsSourceProperty, sourceBinding);
            var indexBinding = new Binding("PageIndex");
            indexBinding.Mode = BindingMode.OneWay;
            BindingOperations.SetBinding(cboPage, ComboBox.SelectedValueProperty, indexBinding);
        }

        private void ClearBindings()
        {
            BindingOperations.ClearAllBindings(cboPage);
        }

        private void TriggerPageIndexChanged(int pageIndex)
        {
            if (PageIndexChanged != null)
            {
                PageIndexChanged(this, new PagerEventArgs{PageIndex = pageIndex});
            }
        }

        public event PagerHandler PageIndexChanged;
    }

    public delegate void PagerHandler(object sender, PagerEventArgs args);

    public class PagerEventArgs : EventArgs
    {
        public int PageIndex { get; set; }
    }
}
