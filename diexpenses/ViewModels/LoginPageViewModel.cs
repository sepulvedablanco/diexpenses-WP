using diexpenses.Services;
using diexpenses.ViewModels.Base;
using System;
using System.Diagnostics;
using System.Windows.Input;
using Windows.UI.Xaml.Navigation;

namespace diexpenses.ViewModels
{
    public class LoginPageViewModel: ViewModelBase
    {
        private string username;
        public static Func<string> PasswordHandler { get; set; }

        private static DelegateCommand loginCommand;
        private static DelegateCommand signupCommand;

        private IDialogService dialogService;
        private INavigationService navigationService;

        public LoginPageViewModel(IDialogService dialogService, INavigationService navigationService)
        {
            this.dialogService = dialogService;
            this.navigationService = navigationService;

            loginCommand = new DelegateCommand(LoginExecute, LoginCanExecute);
            signupCommand = new DelegateCommand(NavigateToSingupExecute, null);

        }
        
        public ICommand LoginCommand
        {
            get { return loginCommand; }
        }

        public ICommand SignupCommand
        {
            get { return signupCommand; }
        }
        
        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                loginCommand.RaiseCanExecuteChanged();
            }
        }

        private void LoginExecute()
        {
            Username = this.username;

            Debug.WriteLine("Username=" + username + ". Password=" + PasswordHandler());

            // Perform login
            DoLogin();
        }

        private bool LoginCanExecute()
        {
            Debug.WriteLine("Password=" + PasswordHandler());

            if (string.IsNullOrEmpty(this.username) || string.IsNullOrEmpty(PasswordHandler())) { return false; }
            return true;
        }

        public override void NavigateTo(NavigationEventArgs e)
        {
            base.NavigateTo(e);

            this.navigationService.AppFrame = base.AppFrame;
        }

        private void NavigateToSingupExecute()
        {
            Debug.WriteLine("Navigating to Signup view");
            // Navigate to signup view
            //this.navigationService.NavegarPaginaDos(this.texto1);
        }

        public void DoLogin()
        {

        }

        public static void OnPasswordChanged()
        {
            loginCommand.RaiseCanExecuteChanged();
        }

    }
}
