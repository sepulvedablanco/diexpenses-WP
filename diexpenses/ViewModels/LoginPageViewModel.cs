namespace diexpenses.ViewModels
{
    using diexpenses.Common;
    using diexpenses.Entities;
    using diexpenses.Services;
    using diexpenses.Services.DiexpensesAPI;
    using diexpenses.Services.NetworkService;
    using diexpenses.ViewModels.Base;
    using System;
    using System.Diagnostics;
    using System.Windows.Input;
    using Views;
    using Windows.UI.Xaml.Navigation;

    public class LoginPageViewModel: ViewModelBase
    {
        private string username;
        public static Func<string> PasswordHandler { get; set; }

        private static DelegateCommand loginCommand;
        private static DelegateCommand signupCommand;

        private IDialogService dialogService;
        private INavigationService navigationService;
        private INetworkService networkService;
        private IApiService apiService;

        public LoginPageViewModel(IDialogService dialogService, INavigationService navigationService, INetworkService networkService, IApiService apiService)
        {
            this.dialogService = dialogService;
            this.navigationService = navigationService;
            this.networkService = networkService;
            this.apiService = apiService;

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
            this.navigationService.NavigateTo<SignupPage>(null);
        }

        public async void DoLogin()
        {
            if(!networkService.IsNetworkAvailable)
            {
                dialogService.ShowAlert("Please, check you Internet connection!");
                return;
            }

            User user = await apiService.Login(Username, PasswordHandler());
            if(user == null)
            {
                dialogService.ShowAlert("Incorrect user or password");
                return;
            }
            Debug.WriteLine(user.ToString());

            Utils.SaveDataInMemory(user);

            this.navigationService.NavigateTo<HomePage>(null);
        }

        public static void OnPasswordChanged()
        {
            loginCommand.RaiseCanExecuteChanged();
        }

    }
}
