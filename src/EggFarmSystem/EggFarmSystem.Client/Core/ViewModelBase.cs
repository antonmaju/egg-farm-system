using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Core
{
    public class ViewModelBase : INotifyPropertyChanged, IDisposable, IDataErrorInfo 
    {
        public event PropertyChangedEventHandler PropertyChanged;
   
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                var args = new PropertyChangedEventArgs(propertyName);
                handler(this, args);
            }

        }

        public virtual void Dispose()
        {
            
        }

        public string Error
        {
            get { return null; }
        }

        public virtual string this[string columnName]
        {
            get { return null; }
        }
    }
}
