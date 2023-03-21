using System;
using System.Windows.Input;

namespace ServerTCP.ViewModels
{
    /// <summary>
    /// A command that allows an action to be executed 
    /// and a function to determine whether the action can be executed. 
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action execute;
        private readonly Func<bool> canExecute;
        /// <summary>
        /// Occurs when changes occur that affect whether the command can be executed.
        /// ICommand property, had to implement.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        /// <summary>
        /// Initializes
        /// </summary>
        /// <param name="execute">The action to execute when the command is executed.</param>
        /// <param name="canExecute">The function to determine whether the command can be executed.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Determines whether the command can be executed.
        /// </summary>
        /// <param name="parameter">The parameter passed to the command.</param>
        /// <returns>true if the command can be executed; otherwise, false.</returns>
        public bool CanExecute(object parameter)
        {
            //If there is no canExecute function, or the canExecute function returns true, return true
            return canExecute == null || canExecute();
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">The parameter passed to the command.</param>
        public void Execute(object parameter)
        {
            // Execute the execute action
            execute();
        }
    }
}
