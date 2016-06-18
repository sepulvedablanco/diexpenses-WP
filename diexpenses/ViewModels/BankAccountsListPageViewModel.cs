namespace diexpenses.ViewModels
{
    using diexpenses.Entities;
    using diexpenses.Services;
    using diexpenses.ViewModels.Base;
    using Services.Database;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Windows.Input;
    using Views;
    using Windows.UI.Xaml.Navigation;

    public class BankAccountsListPageViewModel : MenuBottomViewModelBase
    {
        private ObservableCollection<BankAccount> items;

        private static DelegateCommand newBankAccountCommand;

        private IDbService dbService;
        private IDialogService dialogService;

        public BankAccountsListPageViewModel(IDbService dbService, INavigationService navigationService, IDialogService dialogService) : base(navigationService)
        {
            this.dbService = dbService;
            this.dialogService = dialogService;

            newBankAccountCommand = new DelegateCommand(NewBankAccountExecute, null);

            LoadBankAccounts();
        }

        private void LoadBankAccounts()
        {
            var bankAccountsList = this.dbService.SelectBankAccounts();
            Debug.WriteLine("Number of bank accounts retrieved: " + bankAccountsList.Count);
            Items = new ObservableCollection<BankAccount>(bankAccountsList);
        }

        public ICommand NewBankAccountCommand
        {
            get { return newBankAccountCommand; }
        }

        private void NewBankAccountExecute()
        {
            Debug.WriteLine("NewBankAccountExecute");

            NavigationService.NavigateTo<BankAccountDetailsPage>(null);
        }

        public ObservableCollection<BankAccount> Items
        {
            get { return this.items; }
            set
            {
                this.items = value;
                RaisePropertyChanged();
            }
        }

        public override void NavigateTo(NavigationEventArgs e)
        {
            base.NavigateTo(e);

            this.NavigationService.AppFrame = base.AppFrame;
        }
    }
}
