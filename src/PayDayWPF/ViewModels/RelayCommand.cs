using System;
using System.Windows.Input;

namespace PayDayWPF.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execteMethod;
        private readonly Func<object, bool> _canExecuteMethod;

        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action<object> execteMethod, Func<object, bool> canExecuteMethod = null)
        {
            _execteMethod = execteMethod;
            _canExecuteMethod = canExecuteMethod;
        }

        public bool CanExecute(object? parameter)
        {
            if (_canExecuteMethod != null)
            {
                return _canExecuteMethod(parameter);
            }
            else
            {
                return true;
            }
        }

        public void Execute(object? parameter)
        {
            _execteMethod(parameter);
        }
    }
}