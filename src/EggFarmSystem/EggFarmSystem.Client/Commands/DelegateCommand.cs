using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Commands
{
    public class DelegateCommand : CommandBase
    {
        private Action<object> executeAction;
        private Predicate<object> canExecutePredicate; 

        public DelegateCommand(Action<object> executeAction, Predicate<object> canExecutePredicate)
        {
            this.executeAction = executeAction;
            this.canExecutePredicate = canExecutePredicate;
        }

        public override bool CanExecute(object parameter)
        {
            if (canExecutePredicate != null)
                canExecutePredicate(parameter);

            return base.CanExecute(parameter);
        }

        public override void Execute(object parameter)
        {
            if (executeAction != null)
                executeAction(parameter);
        }
    }

    public class DelegateCommand<T> : CommandBase<T>
    {
        private readonly Action<T> executeCallback;
        private readonly Predicate<T> canExecuteCallback;

        public DelegateCommand(Action<T> executeCallback, Predicate<T> canExecuteCallback)
        {
            this.executeCallback = executeCallback;
            this.canExecuteCallback = canExecuteCallback;
        }

        public override bool CanExecute(T parameter)
        {
            if(canExecuteCallback != null)
                return base.CanExecute(parameter);

            return base.CanExecute(parameter);
        }

        public override void Execute(T parameter)
        {
            if(executeCallback != null)
                executeCallback(parameter);
        }
    }
}
