namespace diexpenses.ViewModels
{
    using diexpenses.Entities;
    using diexpenses.Services;
    using diexpenses.ViewModels.Base;
    using Services.Database;
    using System.Diagnostics;
    using System.Windows.Input;
    using Windows.UI.Xaml.Navigation;

    public class BankAccountDetailsPageViewModel : MenuBottomViewModelBase
    {
        private static BankAccount bankAccount;

        private static DelegateCommand actionCommand;

        private IDbService dbService;
        private IDialogService dialogService;

        public BankAccountDetailsPageViewModel(IDbService dbService, IDialogService dialogService, INavigationService navigationService) : base(navigationService)
        {
            this.dbService = dbService;
            this.dialogService = dialogService;

            actionCommand = new DelegateCommand(ActionExecute, ActionCanExecute);

            if(bankAccount != null) { 
                bankAccount.PropertyChanged += delegate
                {
                    actionCommand.RaiseCanExecuteChanged();
                };
            }
        }

        public ICommand ActionCommand
        {
            get { return actionCommand; }
        }

        public static BankAccount BankAccount
        {
            get { return bankAccount; }
            set
            {
                bankAccount = value;
                actionCommand.RaiseCanExecuteChanged();
            }
        }

        private void ActionExecute()
        {
            Debug.WriteLine("ActionExecute");

            bankAccount.CompleteBankAccount = bankAccount.Iban + bankAccount.Entity + bankAccount.Office + bankAccount.ControlDigit + bankAccount.AccountNumber;
            dbService.Upsert<BankAccount>(bankAccount);

            NavigationService.GoBack();
        }

        private bool ActionCanExecute()
        {
            Debug.WriteLine("ActionCanExecute");
            return IsValidForm();
        }

        private bool IsValidForm()
        {
            bool valid = !string.IsNullOrEmpty(bankAccount.Description);
            valid = !string.IsNullOrEmpty(bankAccount.Iban) && valid;
            valid = !string.IsNullOrEmpty(bankAccount.Entity) && valid;
            valid = !string.IsNullOrEmpty(bankAccount.Office) && valid;
            valid = !string.IsNullOrEmpty(bankAccount.ControlDigit) && valid;
            valid = !string.IsNullOrEmpty(bankAccount.AccountNumber) && valid;
            return valid;
        }

        public override void NavigateTo(NavigationEventArgs e)
        {
            base.NavigateTo(e);

            NavigationService.AppFrame = base.AppFrame;
        }
    }
}
