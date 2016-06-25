namespace diexpenses.ViewModels
{
    using Common;
    using diexpenses.Entities;
    using diexpenses.Services;
    using diexpenses.ViewModels.Base;
    using Services.Database;
    using Services.StorageService;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Windows.Input;
    using Views;
    using Windows.UI.Xaml.Navigation;

    public class BankAccountsListPageViewModel : MenuBottomViewModelBase
    {
        private ObservableCollection<BankAccount> items;

        private static DelegateCommand newBankAccountCommand;
        private static DelegateCommand editBankAccountCommand;
        private static DelegateCommand deleteBankAccountCommand;

        private IDialogService dialogService;

        public BankAccountsListPageViewModel(IDbService dbService, INavigationService navigationService, IDialogService dialogService, IStorageService storageService) : base(navigationService, dbService, storageService)
        {
            this.dialogService = dialogService;

            newBankAccountCommand = new DelegateCommand(NewBankAccountExecute);
            editBankAccountCommand = new DelegateCommand(EditBankAccountExecute);
            deleteBankAccountCommand = new DelegateCommand(DeleteBankAccountExecute);

            LoadBankAccounts();
        }

        private void LoadBankAccounts()
        {
            var bankAccountsList = this.DbService.SelectBankAccounts();
            Debug.WriteLine("Number of bank accounts retrieved: " + bankAccountsList.Count);
            Items = new ObservableCollection<BankAccount>(bankAccountsList);
        }

        public ICommand NewBankAccountCommand
        {
            get { return newBankAccountCommand; }
        }

        public ICommand EditBankAccountCommand
        {
            get { return editBankAccountCommand; }
        }

        public ICommand DeleteBankAccountCommand
        {
            get { return deleteBankAccountCommand; }
        }

        private void NewBankAccountExecute()
        {
            Debug.WriteLine("NewBankAccountExecute");

            NavigationService.NavigateTo(new BankAccountDetailsPage(new BankAccount()));
        }

        private void EditBankAccountExecute()
        {
            Debug.WriteLine("EditBankAccountExecute");
            BankAccount bankAccount = OpenMenuFlyoutAction.HoldedObject as BankAccount;
            if (bankAccount == null)
            {
                Debug.WriteLine("Invalid bank account to edit...");
                return;
            }

            Debug.WriteLine("Bank account to edit: " + bankAccount.ToString());

            NavigationService.NavigateTo(new BankAccountDetailsPage(bankAccount));
        }

        private async void DeleteBankAccountExecute()
        {
            Debug.WriteLine("DeleteBankAccountExecute");
            BankAccount bankAccount = OpenMenuFlyoutAction.HoldedObject as BankAccount;
            if (bankAccount == null)
            {
                Debug.WriteLine("Invalid bank account to delete...");
                return;
            }

            Debug.WriteLine("Bank account to delete: " + bankAccount.ToString());

            bool result = await dialogService.ShowConfirmMessage("Delete bank account", "Are you sure you want to delete the bank account " + bankAccount.Description, "I agree", "Delete", "Cancel");
            Debug.WriteLine("Delete bank account: " + result);
            if (result)
            {
                if (DbService.Delete<BankAccount>(bankAccount))
                {
                    LoadBankAccounts();
                }
            }
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
