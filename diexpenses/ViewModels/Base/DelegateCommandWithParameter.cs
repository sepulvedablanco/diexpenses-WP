namespace diexpenses.ViewModels.Base
{
    using System;
    using System.Windows.Input;

    public class DelegateCommandWithParameter<T> : ICommand
    {
        private Action<T> execute;
        private Func<bool> canExecute;

        public event EventHandler CanExecuteChanged;

        public DelegateCommandWithParameter(Action<T> exec, Func<bool> canExec)
        {
            this.execute = exec;
            this.canExecute = canExec;
        }

        public bool CanExecute(object parameter)
        {
            if (this.canExecute == null) { return true; }

            return this.canExecute();
        }

        public void Execute(object parameter)
        {
            if (this.execute != null)
            {
                var castParameter = (T) Convert.ChangeType(parameter, typeof(T));
                this.execute(castParameter);
            }
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, new EventArgs());
            }
        }
    }
}
