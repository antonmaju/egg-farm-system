using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace EggFarmSystem.Client.Controls
{
    /// <summary>
    /// This approach is inspired by excellent "Sams WPF Control Development Unleashed" book
    /// </summary>
    public class ProgressBarTaskHelper
    {
        private const int TaskCount = 4;

        /// <summary>
        /// This task list should consist of 4 tasks
        /// </summary>
        public static readonly DependencyProperty TaskListProperty = DependencyProperty.RegisterAttached("TaskList",
                                                                                                         typeof (List<ProgressBarTaskInfo>),
                                                                                                         typeof (ProgressBarTaskHelper),
                                                                                                         new PropertyMetadata(OnTaskListChanged));

        public static readonly DependencyPropertyKey TaskStagePropertyKey =
            DependencyProperty.RegisterAttachedReadOnly("TaskStage",
                                                        typeof (TaskStage), typeof (ProgressBarTaskHelper),
                                                        new PropertyMetadata(TaskStage.Stage1));

        public static readonly DependencyProperty TaskStageProperty = TaskStagePropertyKey.DependencyProperty;

        static void OnTaskListChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tasks = e.NewValue as List<ProgressBarTaskInfo>;
            ProgressBar bar = d as ProgressBar;

            if (bar == null) return;

            if (tasks == null || tasks.Count != TaskCount)
            {
                bar.SetValue(TaskStagePropertyKey,TaskStage.Stage1);
                return;
            }

            TaskStage stage = TaskStage.Stage1;
            bool taskStop = false;

            if (tasks[0].Done) stage = TaskStage.Stage2; else taskStop = true;
            if (tasks[1].Done && ! taskStop) stage = TaskStage.Stage3; else taskStop = true;
            if (tasks[2].Done && ! taskStop) stage = TaskStage.Stage4; else taskStop = true;
            if (tasks[3].Done && !taskStop) stage = TaskStage.Stage5;
            


        }

    }

    public enum TaskStage
    {
        Stage1,
        Stage2,
        Stage3,
        Stage4,
        Stage5
    }


    public class ProgressBarTaskInfo
    {
        public string TaskId { get; set; }

        public string Tag { get; set; }

        public string Description { get; set; }

        public bool Done { get; set; }
    }
}
