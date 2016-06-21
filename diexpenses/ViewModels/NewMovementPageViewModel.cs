namespace diexpenses.ViewModels
{
    using diexpenses.Services;
    using diexpenses.ViewModels.Base;
    using Entities;
    using Services.Database;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Windows.Input;
    using Windows.UI.Xaml.Navigation;

    public class NewMovementPageViewModel : MenuBottomViewModelBase
    {
        private ObservableCollection<Kind> kinds;
        private ObservableCollection<Subkind> subkinds;
        private ObservableCollection<BankAccount> bankAccounts;

        private Movement movement = new Movement();

        private static DelegateCommand saveCommand;
        private static DelegateCommand kindChangedCommand;

        private IDbService dbService;
        private IDialogService dialogService;

        public NewMovementPageViewModel(IDbService dbService, IDialogService dialogService, INavigationService navigationService) : base(navigationService)
        {
            this.dbService = dbService;
            this.dialogService = dialogService;

            saveCommand = new DelegateCommand(SaveExecute, SaveCanExecute);
            kindChangedCommand = new DelegateCommand(KindChangedExecute, null);

            LoadKinds();
            LoadBankAccounts();

            movement.PropertyChanged += delegate
            {
                saveCommand.RaiseCanExecuteChanged();
            };
        }

        public ICommand SaveCommand
        {
            get { return saveCommand; }
        }

        public ICommand KindChangedCommand
        {
            get { return kindChangedCommand; }
        }

        private void LoadKinds()
        {
            var kindsList = this.dbService.SelectKinds();
            Debug.WriteLine("Number of kinds retrieved: " + kindsList.Count);
            Kinds = new ObservableCollection<Kind>(kindsList);
            if(Kinds.Count > 0)
            {
                Movement.Kind = Kinds[0];
                LoadSubkinds(Movement.KindId);
            }
        }

        private void LoadSubkinds(int kindId)
        {
            if(kindId == -1)
            {
                Debug.WriteLine("Invalid kindId. Subkind search won't be executed.");
                return;
            }
            var subkindsList = this.dbService.SelectSubkinds(kindId);
            Debug.WriteLine("Number of subkinds retrieved: " + subkindsList.Count);
            Subkinds = new ObservableCollection<Subkind>(subkindsList);
            if (Subkinds.Count > 0)
            {
                Movement.Subkind = Subkinds[0];
            }
        }

        private void LoadBankAccounts()
        {
            var bankAccountsList = this.dbService.SelectBankAccounts();
            Debug.WriteLine("Number of bank accounts retrieved: " + bankAccountsList.Count);
            BankAccounts = new ObservableCollection<BankAccount>(bankAccountsList);
            if (bankAccountsList.Count > 0)
            {
                Movement.BankAccount = bankAccountsList[0];
            }
        }

        private void SaveExecute()
        {
            Debug.WriteLine("SaveExecute");
            Debug.WriteLine("Movement to save: " + Movement.ToString());
            dbService.Upsert<Movement>(movement);
            NavigationService.GoBack();
        }

        private bool SaveCanExecute()
        {
            Debug.WriteLine("SaveCanExecute");
            return IsValidForm();
        }

        private bool IsValidForm()
        {
            bool valid = !string.IsNullOrEmpty(Movement.Concept);
            valid = Movement.KindId != -1 && valid;
            valid = Movement.SubkindId != -1 && valid;
            valid = Movement.BankAccountId != -1 && valid;
            valid = Movement.TransactionDate != null && valid;
            return valid;
        }

        private void KindChangedExecute()
        {
            Debug.WriteLine("KindChangedExecute");
            LoadSubkinds(Movement.KindId);
        }

        public ObservableCollection<Kind> Kinds
        {
            get { return this.kinds; }
            set
            {
                this.kinds = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Subkind> Subkinds
        {
            get { return this.subkinds; }
            set
            {
                this.subkinds = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<BankAccount> BankAccounts
        {
            get { return this.bankAccounts; }
            set
            {
                this.bankAccounts = value;
                RaisePropertyChanged();
            }
        }

        public Movement Movement
        {
            get { return this.movement; }
            set
            {
                this.movement = value;
                saveCommand.RaiseCanExecuteChanged();
            }
        }

        public override void NavigateTo(NavigationEventArgs e)
        {
            base.NavigateTo(e);

            NavigationService.AppFrame = base.AppFrame;
        }
    }
}
