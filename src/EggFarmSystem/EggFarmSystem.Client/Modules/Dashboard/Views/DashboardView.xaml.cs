﻿using System;
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
using EggFarmSystem.Client.Core.Views;
using EggFarmSystem.Client.Modules.Dashboard.ViewModels;

namespace EggFarmSystem.Client.Modules.Dashboard.Views
{
    /// <summary>
    /// Interaction logic for DashboardView.xaml
    /// </summary>
    public partial class DashboardView : UserControlBase, IDashboardView
    {
        private DashboardViewModel model;

        public DashboardView(DashboardViewModel model)
        {
            InitializeComponent();
            this.model = model;
            this.DataContext = model;
        }

        public override void Dispose()
        {
            model.Dispose();
            base.Dispose();
        }

    }

    public interface IDashboardView
    {
        
    }
}
