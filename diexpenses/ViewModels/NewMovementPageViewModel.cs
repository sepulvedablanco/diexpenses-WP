namespace diexpenses.ViewModels
{
    using diexpenses.Services;
    using diexpenses.ViewModels.Base;
    using Entities;
    using Services.Database;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using Windows.UI.Xaml.Navigation;

    public class NewMovementPageViewModel : MenuBottomViewModelBase
    {
        private ObservableCollection<Kind> kinds;
        private ObservableCollection<Subkind> subkinds;
        private ObservableCollection<BankAccount> bankAccounts;

        private Kind kindSelected;
        private Subkind subkindSelected;
        private BankAccount bankAccountSelected;

        private static DelegateCommand saveCommand;

        private IDbService dbService;
        private IDialogService dialogService;

        public NewMovementPageViewModel(IDbService dbService, IDialogService dialogService, INavigationService navigationService) : base(navigationService)
        {
            this.dbService = dbService;
            this.dialogService = dialogService;

            saveCommand = new DelegateCommand(SaveExecute, SaveCanExecute);

            LoadKinds();
            LoadBankAccounts();
        }

        private void LoadKinds()
        {
            var kindsList = this.dbService.SelectKinds();
            Debug.WriteLine("Number of kinds retrieved: " + kindsList.Count);
            Kinds = new ObservableCollection<Kind>(kindsList);
            if(Kinds.Count > 0)
            {
                KindSelected = Kinds[0];
                LoadSubkinds(kindSelected.Id.GetValueOrDefault(-1));
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
                SubkindSelected = Subkinds[0];
            }
        }

        private void LoadBankAccounts()
        {
            var bankAccountsList = this.dbService.SelectBankAccounts();
            Debug.WriteLine("Number of bank accounts retrieved: " + bankAccountsList.Count);
            BankAccounts = new ObservableCollection<BankAccount>(bankAccountsList);
            if (bankAccountsList.Count > 0)
            {
                BankAccountSelected = bankAccountsList[0];
            }
        }

        private void SaveExecute()
        {
            Debug.WriteLine("SaveExecute");
        }

        private bool SaveCanExecute()
        {
            Debug.WriteLine("SaveCanExecute");
            return true;
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

        public Kind KindSelected
        {
            get { return this.kindSelected; }
            set
            {
                this.kindSelected = value;
                RaisePropertyChanged();
            }
        }

        public Subkind SubkindSelected
        {
            get { return this.subkindSelected; }
            set
            {
                this.subkindSelected = value;
                RaisePropertyChanged();
            }
        }

        public BankAccount BankAccountSelected
        {
            get { return this.bankAccountSelected; }
            set
            {
                this.bankAccountSelected = value;
                RaisePropertyChanged();
            }
        }

        public override void NavigateTo(NavigationEventArgs e)
        {
            base.NavigateTo(e);

            NavigationService.AppFrame = base.AppFrame;
        }
    }
}
