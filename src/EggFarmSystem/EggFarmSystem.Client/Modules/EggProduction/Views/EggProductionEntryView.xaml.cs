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
using EggFarmSystem.Client.Modules.EggProduction.ViewModels;

namespace EggFarmSystem.Client.Modules.EggProduction.Views
{
    /// <summary>
    /// Interaction logic for EggProductionEntryView.xaml
    /// </summary>
    public partial class EggProductionEntryView : UserControlBase, IEggProductionEntryView
    {
        private EggProductionEntryViewModel model;

        public EggProductionEntryView(EggProductionEntryViewModel model)
        {
            InitializeComponent();
            this.model = model;
            this.DataContext = model;
            this.NavigationCommands = model.NavigationCommands;
        }

        public override void Dispose()
        {
            model.Dispose();
            base.Dispose();
        }

    }

    public interface IEggProductionEntryView
    {

    }
}
