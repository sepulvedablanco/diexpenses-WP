namespace diexpenses.ViewModels.Base
{
    using Common;
    using Services;
    using System.Diagnostics;
    using System.Windows.Input;
    using Views;
    using Windows.UI.Xaml.Navigation;

    public class MenuBottomViewModelBase : ViewModelBase
    {
        private static DelegateCommand homeCommand;
        private static DelegateCommand movementsCommand;
        private static DelegateCommand kindsCommand;
        private static DelegateCommand bankAccountsCommand;
        private static DelegateCommand logoutCommand;

        private INavigationService navigationService;

        public MenuBottomViewModelBase(INavigationService navigationService)
        {
            this.navigationService = navigationService;

            homeCommand = new DelegateCommand(HomeExecute, null);
            movementsCommand = new DelegateCommand(MovementsExecute, null);
            kindsCommand = new DelegateCommand(KindsExecute, null);
            bankAccountsCommand = new DelegateCommand(BankAccountsExecute, null);
            logoutCommand = new DelegateCommand(LogoutExecute, null);
        }

        public INavigationService NavigationService
        {
            get { return navigationService; }
        }

        public ICommand HomeCommand
        {
            get { return homeCommand; }
        }

        public ICommand MovementsCommand
        {
            get { return movementsCommand; }
        }

        public ICommand KindsCommand
        {
            get { return kindsCommand; }
        }

        public ICommand BankAccountsCommand
        {
            get { return bankAccountsCommand; }
        }

        public ICommand LogoutCommand
        {
            get { return logoutCommand; }
        }

        public override void NavigateTo(NavigationEventArgs e)
        {
            base.NavigateTo(e);

            this.navigationService.AppFrame = base.AppFrame;
        }

        private void HomeExecute()
        {
            Debug.WriteLine("Home execute...");

            navigationService.NavigateTo<HomePage>(null);
        }

        private void MovementsExecute()
        {
            Debug.WriteLine("Movements execute...");

        }

        private void KindsExecute()
        {
            Debug.WriteLine("Kinds execute...");

            navigationService.NavigateTo<KindsListPage>(null);
        }

        private void BankAccountsExecute()
        {
            Debug.WriteLine("Bank accounts execute...");

            navigationService.NavigateTo<BankAccountsListPage>(null);
        }

        private void LogoutExecute()
        {
            Debug.WriteLine("Logout execute...");

            Utils.DeleteDataInMemory();

            navigationService.NavigateTo<LoginPage>(null);
        }
    }
}
