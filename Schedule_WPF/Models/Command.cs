using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Schedule_WPF.Models
{
    public class Command : ICommand
    {
        private Action<object> execution;

        private Func<object, bool> canExecution;

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public Command(Action<object> execution, Func<object, bool> canExecution) {
            this.execution = execution;
            this.canExecution = canExecution;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecution == null || this.canExecution(parameter); 
        }

        public void Execute(object parameter)
        {
            this.execution(parameter);
        }
    }
}
