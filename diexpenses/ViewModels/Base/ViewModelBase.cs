namespace diexpenses.ViewModels.Base
{
    using Common;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    public class ViewModelBase : INotifyPropertyChanged
    {
        private Frame appFrame;
        private bool showBackButton;
        private DelegateCommand backButtonCommand;

        public ViewModelBase()
        {
            this.backButtonCommand = new DelegateCommand(BackButtonExecute);

            IsBackButtonHardwarePressent();
        }

        public Frame AppFrame
        {
            get { return this.appFrame; }
            set { this.appFrame = value; }
        }

        public ICommand BackButtonCommand
        {
            get { return backButtonCommand; }
        }

        public bool ShowBackButton
        {
            get { return showBackButton; }
            set
            {
                showBackButton = value;
                RaisePropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public virtual void NavigateTo(NavigationEventArgs e) { }
        public virtual void NavigateFrom(NavigationEventArgs e) { }
        public virtual void BackButtonExecute()
        {
            if (this.AppFrame.CanGoBack)
                this.appFrame.GoBack();
        }

        private void IsBackButtonHardwarePressent()
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent(CommonKeys.HARDWARE_BUTTONS_KEY) == true)
            {
                ShowBackButton = false;
            }
            else
            {
                ShowBackButton = true;
            }
        }
    }
}
