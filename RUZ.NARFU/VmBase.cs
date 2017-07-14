using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RUZ.NARFU
{
    class VmBase
    {
        protected string tableLink;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void Set<T>(ref T field, T value, [CallerMemberName] string propName = null)
        {
            if (field != null && !field.Equals(value) || value != null && !value.Equals(field))
            {
                field = value;
                if (PropertyChanged != null)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propName));
            }
        }

    }

    public class Command : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly Action<object> action;

        public Command(Action<object> action)
        {
            this.action = action;
        }
    
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            action(parameter);
        }

    }
       

    

    

}
