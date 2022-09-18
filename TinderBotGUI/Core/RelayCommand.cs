using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TinderBotGUI.Core
{
    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> Execute, Func<object, bool> CanExecute)
        {
            execute = Execute;
            canExecute = CanExecute;
        }

        public bool CanExecute(object param)
        {
            return canExecute == null || canExecute(param);
        }

        public void Execute(object param)
        {
            execute(param);
        }
    }
}
