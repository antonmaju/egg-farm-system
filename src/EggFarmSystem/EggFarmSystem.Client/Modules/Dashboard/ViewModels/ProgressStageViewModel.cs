using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            SetEventHandlers();
        }

        public List<ProgressBarTaskInfo> TaskList
        {
            get { return taskList; }
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
            TaskList = new List<ProgressBarTaskInfo>()
                {
                    new ProgressBarTaskInfo{TaskId="EmployeeCost", Description = "Manages employee cost", Done = false},
                    new ProgressBarTaskInfo{TaskId="Usage", Description = "Manages usage", Done = false},
                    new ProgressBarTaskInfo{TaskId="EggProduction", Description = "Manages egg production", Done = false},
                    new ProgressBarTaskInfo{TaskId="HenDepreciation", Description = "Manages hen depreciation", Done = false}
                };
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
