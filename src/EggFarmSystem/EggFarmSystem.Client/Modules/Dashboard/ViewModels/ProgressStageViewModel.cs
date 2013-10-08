using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using EggFarmSystem.Client.Controls;
using EggFarmSystem.Client.Core;

namespace EggFarmSystem.Client.Modules.Dashboard.ViewModels
{
    public class ProgressStageViewModel : ViewModelBase
    {
        private readonly IMessageBroker broker;

        private List<ProgressBarTaskInfo> taskList;

        public ProgressStageViewModel(IMessageBroker broker)
        {
            this.broker = broker;

            //TODO: change icons

            TaskList = new List<ProgressBarTaskInfo>()
                {
                    new ProgressBarTaskInfo{
                        TaskId="EmployeeCost", 
                        Description = "Manages employee cost", 
                        Done = true, 
                        Image = new BitmapImage(new Uri(@"/EggFarmSystem.Client;component/Assets/Images/employee-menu.png", UriKind.Relative))
                    },
                    new ProgressBarTaskInfo{
                        TaskId="Usage", 
                        Description = "Manages usage", 
                        Done = false,
                        Image =new BitmapImage(new Uri(@"/EggFarmSystem.Client;component/Assets/Images/cost-menu.png", UriKind.Relative))
                    },
                    new ProgressBarTaskInfo
                        {
                            TaskId="EggProduction", 
                            Description = "Manages egg production", 
                            Done = false,
                            Image =new BitmapImage(new Uri(@"/EggFarmSystem.Client;component/Assets/Images/house-menu.png", UriKind.Relative))
                        },
                    new ProgressBarTaskInfo
                        {
                            TaskId="HenDepreciation", 
                            Description = "Manages hen depreciation", 
                            Done = false,
                            Image =new BitmapImage(new Uri(@"/EggFarmSystem.Client;component/Assets/Images/hen-menu.png", UriKind.Relative))
                        }
                };

            SetEventHandlers();
        }

        public List<ProgressBarTaskInfo> TaskList
        {
            get
            {
                return taskList;
            }
            private set 
            { 
                taskList = value;
                OnPropertyChanged("TaskList");
            }
        }

        private void SetEventHandlers()
        {
            broker.Subscribe(DashboardMessages.DateChanged, OnDateChanged);
        }

        void OnDateChanged(object param)
        {
           
        }

        private void UnsetEventHandlers()
        {
            broker.Unsubscribe(DashboardMessages.DateChanged, OnDateChanged);
        }

        public override void Dispose()
        {
            UnsetEventHandlers();
            base.Dispose();
        }
    }
}
