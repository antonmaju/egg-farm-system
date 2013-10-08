using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.Dashboard.Views;

namespace EggFarmSystem.Client.Modules.Dashboard.ViewModels
{
    public class DashboardViewModel : ViewModelBase 
    {
        
         public DashboardViewModel(IProgressStageView stageView)
         {
             StageView = stageView as UserControl;
         }

         public UserControl StageView { get; private set; }


    }
}
