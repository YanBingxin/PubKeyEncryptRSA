using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace PubKeyEncryptRSA.Common
{
    public class RelayCommand : ICommand
    {
        private Action<object> m_Execute;
        private Func<object, bool> m_CanExecute;

        public RelayCommand(Action<object> execute)
        {
            m_Execute = execute;
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canexecute)
        {
            m_Execute = execute;
            m_CanExecute = canexecute;
        }

        public bool CanExecute(object parameter)
        {
            if (m_CanExecute != null)
                return m_CanExecute(parameter);
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (m_Execute != null)
                m_Execute(parameter);
        }
    }
}
