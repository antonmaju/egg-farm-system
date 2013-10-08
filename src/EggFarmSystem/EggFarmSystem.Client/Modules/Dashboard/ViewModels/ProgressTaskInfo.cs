using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using EggFarmSystem.Client.Core;

namespace EggFarmSystem.Client.Modules.Dashboard.ViewModels
{
    public class ProgressBarTaskInfo : ViewModelBase
    {
        private string taskId;
        private string tag;
        private string description;
        private bool done;
        private ImageSource image;

        public string TaskId 
        {
            get { return taskId; }
            set 
            { 
                taskId = value;
                OnPropertyChanged("TaskId");
            }
        }

        public string Tag 
        { 
            get { return tag; } 
            set 
            { 
                tag = value;
                OnPropertyChanged("Tag");
            } 
        }

        public string Description
        {
            get { return description; }
            set
            {
                description = value; 
                OnPropertyChanged("Description");
            }
        }

        public bool Done
        {
            get { return done; } 
            set 
            { 
                done = value;
                OnPropertyChanged("Done");
            }
        }

        public ImageSource Image
        {
            get { return image; }
            set { image = value; OnPropertyChanged("Image"); }
        }
    }
}
