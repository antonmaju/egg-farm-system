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
        public Pager()
        {
            InitializeComponent();
            SetCommandBindings();
        }

        public IPagingInfo PagingSource { get; set; }

        private void SetCommandBindings()
        {
            
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
