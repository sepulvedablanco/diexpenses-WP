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

    public class SignupPageViewModel: ViewModelBase
    {
        private string name;
        private string username;

        public static Func<string> PasswordHandler { get; set; }
        public static Func<string> ConfirmPasswordHandler { get; set; }

        private static DelegateCommand createAccountCommand;
        private static DelegateCommand loginCommand;

        private IDialogService dialogService;
        private INavigationService navigationService;
        private INetworkService networkService;
        private IApiService apiService;

        public SignupPageViewModel(IDialogService dialogService, INavigationService navigationService, INetworkService networkService, IApiService apiService)
        {
            this.dialogService = dialogService;
            this.navigationService = navigationService;
            this.networkService = networkService;
            this.apiService = apiService;

            createAccountCommand = new DelegateCommand(CreateAccountExecute, CreateAccountCanExecute);
            loginCommand = new DelegateCommand(NavigateToLoginExecute);
        }
        
        public ICommand CreateAccountCommand
        {
            get { return createAccountCommand; }
        }

        public ICommand LoginCommand
        {
            get { return loginCommand; }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                loginCommand.RaiseCanExecuteChanged();
            }
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

        private void CreateAccountExecute()
        {
            Name = this.name;
            Username = this.username;

            Debug.WriteLine("Name=" + Name + ". Username=" + Username + ". Password=" + PasswordHandler() + ". Confirm Password=" + ConfirmPasswordHandler());

            if(!PasswordHandler().Equals(ConfirmPasswordHandler()))
            {
                dialogService.ShowAlert("Passwords does not match!");
                return;
            }

            CreateUser();
        }

        private bool CreateAccountCanExecute()
        {
            Debug.WriteLine("Password=" + PasswordHandler() + ". Confirm password=" + ConfirmPasswordHandler());

            if (string.IsNullOrEmpty(this.name) || string.IsNullOrEmpty(this.username) || 
                string.IsNullOrEmpty(PasswordHandler()) || string.IsNullOrEmpty(ConfirmPasswordHandler())) { return false; }
            return true;
        }

        public override void NavigateTo(NavigationEventArgs e)
        {
            base.NavigateTo(e);

            this.navigationService.AppFrame = base.AppFrame;
        }
        
        private void NavigateToLoginExecute()
        {
            Debug.WriteLine("Navigating to Login view");
            this.navigationService.NavigateTo<LoginPage>(null);
        }

        public async void CreateUser()
        {
            if (!networkService.IsNetworkAvailable)
            {
                dialogService.ShowAlert("Please, check you Internet connection!");
                return;
            }

            User user = await apiService.Register(Name, Username, PasswordHandler());
            if (user == null)
            {
                dialogService.ShowAlert("Username in use or password does not contains digits and upper and lower letters");
                return;
            }
            Debug.WriteLine(user.ToString());

            Utils.SaveUserDataInMemory(user);

            this.navigationService.NavigateTo<HomePage>(null);
        }

        public static void OnPasswordChanged()
        {
            createAccountCommand.RaiseCanExecuteChanged();
        }

        public static void OnConfirmPasswordChanged()
        {
            createAccountCommand.RaiseCanExecuteChanged();
        }
    }
}
