namespace diexpenses.ViewModels
{
    using Common;
    using diexpenses.Entities;
    using diexpenses.Services;
    using diexpenses.ViewModels.Base;
    using Services.Database;
    using Services.StorageService;
    using System.Diagnostics;
    using System.Text.RegularExpressions;
    using System.Windows.Input;
    using Windows.UI.Xaml.Navigation;

    public class BankAccountDetailsPageViewModel : MenuBottomViewModelBase
    {
        private static BankAccount bankAccount;

        private static DelegateCommand actionCommand;

        private IDialogService dialogService;

        public BankAccountDetailsPageViewModel(IDbService dbService, IDialogService dialogService, INavigationService navigationService, IStorageService storageService) : base(navigationService, dbService, storageService)
        {
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
            DbService.Upsert<BankAccount>(bankAccount);

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
            valid = valid && !string.IsNullOrEmpty(bankAccount.Iban);
            valid = valid && !string.IsNullOrEmpty(bankAccount.Entity);
            valid = valid && !string.IsNullOrEmpty(bankAccount.Office);
            valid = valid && !string.IsNullOrEmpty(bankAccount.ControlDigit);
            valid = valid && !string.IsNullOrEmpty(bankAccount.AccountNumber);

            if(!valid)
            {
                return false;
            }

            valid = valid && ValidateIban(bankAccount.Iban);
            valid = valid && Validate(bankAccount.Entity, 4, "Entity must consist of four digits");
            valid = valid && Validate(bankAccount.Office, 4, "Office must consist of four digits");
            valid = valid && Validate(bankAccount.ControlDigit, 2, "Control digit must consist of two digits");
            valid = valid && Validate(bankAccount.AccountNumber, 10, "Account number must consist of ten digits");

            return valid;
        }

        public bool ValidateIban(string iban)
        {
            bool valid = iban.Length == 4;
            valid = valid && Regex.IsMatch(iban.Substring(0,2), @"^[a-zA-Z]+$");
            valid = valid && Regex.IsMatch(iban.Substring(2, 2), @"^[0-9]+$");
            if(!valid)
            {
                dialogService.ShowAlert("Iban must contains two chars and two digits");
            }
            return valid;
        }

        public bool Validate(string value, int length, string message)
        {
            if (!Utils.IsValidNumber(value, length))
            {
                dialogService.ShowAlert(message);
                return false;
            }
            return true;
        }

        public override void NavigateTo(NavigationEventArgs e)
        {
            base.NavigateTo(e);

            NavigationService.AppFrame = base.AppFrame;
        }
    }
}
