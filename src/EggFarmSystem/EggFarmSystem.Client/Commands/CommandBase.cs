using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace EggFarmSystem.Client.Commands
{
    /// <summary>
    /// Represents base class for commands in this applications
    /// </summary>
    public abstract class CommandBase : ICommand
    {
        /// <summary>
        /// Gets or sets the command id.
        /// </summary>
        /// <value>The id.</value>
        public virtual string Id { get; protected set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text retrieval function.</value>
        public virtual Func<string> Text { get; protected set; }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
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
            T param = default(T);
            if (parameter != null)
                param = (T) parameter;

            return this.CanExecute(param);
        }

        public override void Execute(object parameter)
        {
            T param = default(T);
            if (parameter != null)
                param = (T)parameter;

            Execute((T) param);
        }
    }
}
