namespace diexpenses.ViewModels.Base
{
    using common.Common;
    using Services;
    using System;
    using System.Diagnostics;
    using System.Windows.Input;
    using Views;
    using Windows.UI.StartScreen;
    using Windows.UI.Xaml.Navigation;
    using System.Collections.Generic;
    using common.Services.Database;
    using Services.StorageService;
    using common.Tiles;

    public class MenuBottomViewModelBase : ViewModelBase
    {
        private bool isPinned;

        private static DelegateCommand homeCommand;
        private static DelegateCommand movementsCommand;
        private static DelegateCommand kindsCommand;
        private static DelegateCommand bankAccountsCommand;
        private static DelegateCommand aboutCommand;
        private static DelegateCommand managePinCommand;
        private static DelegateCommand logoutCommand;

        private INavigationService navigationService;
        private IDbService dbService;
        private IStorageService storageService;

        public MenuBottomViewModelBase(INavigationService navigationService, IDbService dbService, IStorageService storageService)
        {
            this.navigationService = navigationService;
            this.dbService = dbService;
            this.storageService = storageService;

            string tileId = Utils.GetTileId();
            isPinned = tileId != null && SecondaryTile.Exists(tileId);

            homeCommand = new DelegateCommand(HomeExecute);
            movementsCommand = new DelegateCommand(MovementsExecute);
            kindsCommand = new DelegateCommand(KindsExecute);
            bankAccountsCommand = new DelegateCommand(BankAccountsExecute);
            aboutCommand = new DelegateCommand(AboutExecute);
            managePinCommand = new DelegateCommand(ManagePinExecute);
            logoutCommand = new DelegateCommand(LogoutExecute);
        }

        public INavigationService NavigationService
        {
            get { return navigationService; }
        }

        public IDbService DbService
        {
            get { return dbService; }
        }

        public IStorageService StorageService
        {
            get { return storageService; }
        }

        public bool IsPinned
        {
            get { return isPinned; }
            set
            {
                isPinned = value;
                RaisePropertyChanged();
            }
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

        public ICommand AboutCommand
        {
            get { return aboutCommand; }
        }

        public ICommand ManagePinCommand
        {
            get { return managePinCommand; }
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

            navigationService.NavigateTo<MovementsListPage>(null);
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

        private void AboutExecute()
        {
            Debug.WriteLine("About execute...");

            navigationService.NavigateTo<AboutPage>(null);
        }

        private void ManagePinExecute()
        {
            if (IsPinned)
            {
                UnpinFromStart();
            }
            else
            {
                PinToStart();
            }
        }

        private async void UnpinFromStart() {
            Utils.DeleteTileId();

            IReadOnlyList<SecondaryTile> lstTiles = await SecondaryTile.FindAllForPackageAsync();
            if(lstTiles == null || lstTiles.Count == 0)
            {
                IsPinned = false;
                return;
            }
            SecondaryTile secondaryTile = lstTiles[0];
            bool result = await secondaryTile.RequestDeleteAsync();
            if (result)
            {
                IsPinned = false;
            }
        }

        private async void PinToStart()
        {
            IsPinned = await TileGenerator.Generate(dbService, true);
        }

        private void LogoutExecute()
        {
            Debug.WriteLine("Logout execute...");

            Common.Utils.DeleteUserDataInMemory();

            navigationService.NavigateTo<LoginPage>(null);
        }
    }
}
