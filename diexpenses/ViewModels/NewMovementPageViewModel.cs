namespace diexpenses.ViewModels
{
    using diexpenses.Services;
    using diexpenses.ViewModels.Base;
    using Entities;
    using Services.Database;
    using Services.GpsService;
    using Services.StorageService;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Windows.Devices.Geolocation;
    using Windows.UI.Xaml.Navigation;

    public class NewMovementPageViewModel : MenuBottomViewModelBase
    {
        private ObservableCollection<Kind> kinds;
        private ObservableCollection<Subkind> subkinds;
        private ObservableCollection<BankAccount> bankAccounts;

        private Movement movement = new Movement();
        private bool previousAlert = false;

        private static DelegateCommand saveCommand;
        private static DelegateCommand kindChangedCommand;

        private IDialogService dialogService;
        private IGpsService gpsService;

        public NewMovementPageViewModel(IDialogService dialogService, IGpsService gpsService, INavigationService navigationService, IDbService dbService, IStorageService storageService) : base(navigationService, dbService, storageService)
        {
            this.dialogService = dialogService;
            this.gpsService = gpsService;

            saveCommand = new DelegateCommand(SaveExecute, SaveCanExecute);
            kindChangedCommand = new DelegateCommand(KindChangedExecute);

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
            var kindsList = this.DbService.SelectKinds();
            Debug.WriteLine("Number of kinds retrieved: " + kindsList.Count);
            Kinds = new ObservableCollection<Kind>(kindsList);
            if(Kinds.Count > 0)
            {
                Movement.Kind = Kinds[0];
                LoadSubkinds(Movement.KindId);
            } else if (Kinds.Count == 0)
            {
                previousAlert = true;
                dialogService.ShowAlert("You have to create types of expenses in order to create a movement");
            }
        }

        private void LoadSubkinds(int kindId)
        {
            if(kindId == -1)
            {
                Debug.WriteLine("Invalid kindId. Subkind search won't be executed.");
                return;
            }
            var subkindsList = this.DbService.SelectSubkinds(kindId);
            Debug.WriteLine("Number of subkinds retrieved: " + subkindsList.Count);
            Subkinds = new ObservableCollection<Subkind>(subkindsList);
            if (Subkinds.Count > 0)
            {
                Movement.Subkind = Subkinds[0];
            } else if (Subkinds.Count == 0)
            {
                previousAlert = true;
                dialogService.ShowAlert("You have to create subtypes of expenses in order to create a movement");
            }
        }

        private void LoadBankAccounts()
        {
            var bankAccountsList = this.DbService.SelectBankAccounts();
            Debug.WriteLine("Number of bank accounts retrieved: " + bankAccountsList.Count);
            BankAccounts = new ObservableCollection<BankAccount>(bankAccountsList);
            if (BankAccounts.Count > 0)
            {
                Movement.BankAccount = bankAccountsList[0];
            } else if (BankAccounts.Count == 0 && !previousAlert)
            {
                dialogService.ShowAlert("You have to create bank accounts in order to create a movement");
            }
        }

        private async void SaveExecute()
        {
            Debug.WriteLine("SaveExecute");
            Debug.WriteLine("Movement to save: " + Movement.ToString());

            Geoposition currentPosition = await GetCurrentPositionExecute();

            if(currentPosition != null)
            {
                Movement.Location = new Geopoint(new BasicGeoposition() { Latitude = currentPosition.Coordinate.Latitude, Longitude = currentPosition.Coordinate.Longitude }); ;
                Debug.WriteLine("Current point: {" + Movement.Latitude + ", " + Movement.Longitude + "}");
            }

            DbService.Upsert<Movement>(movement);
            NavigationService.GoBack();
        }

        private async Task<Geoposition> GetCurrentPositionExecute()
        {
            return await this.gpsService.GetCurrentGeoposition();
        }

        private bool SaveCanExecute()
        {
            Debug.WriteLine("SaveCanExecute");
            return IsValidForm();
        }

        private bool IsValidForm()
        {
            bool valid = !string.IsNullOrEmpty(Movement.Concept);
            valid = Movement.Amount > 0 && valid;
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
