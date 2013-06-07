using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace EggFarmSystem.Client.Commands
{
    public abstract class CommandBase : ICommand
    {
        public virtual string Id { get; protected set; }

        public virtual Func<string> Text { get; protected set; } 

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public abstract void Execute(object parameter);
    }

    public abstract class CommandBase<T> : CommandBase
    {
        public virtual bool CanExecute(T parameter)
        {
            return true;
        }

        public abstract void Execute(T parameter);

        public override bool CanExecute(object parameter)
        {
            return this.CanExecute((T)parameter);
        }

        public override void Execute(object parameter)
        {
           Execute((T) parameter);
        }
    }
}
