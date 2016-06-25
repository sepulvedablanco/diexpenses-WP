namespace diexpenses.ViewModels.Base
{
    using Common;
    using Services;
    using System;
    using System.Diagnostics;
    using System.Windows.Input;
    using Views;
    using Windows.UI.StartScreen;
    using Windows.UI.Xaml.Navigation;
    using Windows.UI.Notifications;
    using NotificationsExtensions.Tiles;
    using System.Collections.Generic;

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

        public MenuBottomViewModelBase(INavigationService navigationService)
        {
            this.navigationService = navigationService;

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
            string tileId = Utils.GetTileId();

            IReadOnlyList<SecondaryTile> lstTiles = await SecondaryTile.FindAllForPackageAsync();
            SecondaryTile secondaryTile = lstTiles[0];
            bool result = await secondaryTile.RequestDeleteAsync();
            if (result)
            {
                IsPinned = false;
            }
        }

        private async void PinToStart()
        {
            string tileId = Guid.NewGuid().ToString();
            SecondaryTile tile = new SecondaryTile(tileId.ToString(), "diexpenses", "tileArgs", new Uri("ms-appx:///Assets/Wide310x150Logo.png"), TileSize.Wide310x150);
            tile.VisualElements.Wide310x150Logo = new Uri("ms-appx:///Assets/Wide310x150Logo.png");
            tile.VisualElements.Square310x310Logo = new Uri("ms-appx:///Assets/Wide310x150Logo.png");
            var result = await tile.RequestCreateAsync();
            if (result)
            {
                var content = new TileContent()
                {
                    Visual = new TileVisual()
                    {
                        TileSmall = new TileBinding()
                        {
                            Content = new TileBindingContentAdaptive()
                            {
                                BackgroundImage = new TileBackgroundImage()
                                {
                                    Source = "https://wiki.jenkins-ci.org/download/attachments/2916393/logo.png"
                                }
                            }
                        },
                        TileMedium = new TileBinding()
                        {
                            Content = new TileBindingContentPhotos()
                            {
                                Images = {
                                    new TileImageSource("https://wiki.jenkins-ci.org/download/attachments/2916393/logo.png"),
                                    new TileImageSource("http://was.www.praqma.com/sites/default/files/img/cool-jenkins2x3.png"),
                                    new TileImageSource("http://qvacua.com/media/jenkins-menu-icon.png")
                                }
                            }
                        },
                        TileWide = new TileBinding()
                        {
                            Content = new TileBindingContentAdaptive()
                            {
                                BackgroundImage = new TileBackgroundImage()
                                {
                                    Source = "https://wiki.jenkins-ci.org/download/attachments/2916393/logo.png"
                                }
                            }
                        },
                        TileLarge = new TileBinding()
                        {
                            Content = new TileBindingContentPhotos()
                            {
                                Images = {
                                    new TileImageSource("https://wiki.jenkins-ci.org/download/attachments/2916393/logo.png"),
                                    new TileImageSource("http://was.www.praqma.com/sites/default/files/img/cool-jenkins2x3.png"),
                                    new TileImageSource("http://qvacua.com/media/jenkins-menu-icon.png")
                                }
                            }
                        }
                    }
                };

                var xml = content.GetXml();
                TileUpdateManager.CreateTileUpdaterForSecondaryTile(tileId.ToString()).Update(new TileNotification(xml));
                Utils.SaveTileIdInMemory(tileId);
                IsPinned = true;
            }
        }

        private void LogoutExecute()
        {
            Debug.WriteLine("Logout execute...");

            Utils.DeleteUserDataInMemory();

            navigationService.NavigateTo<LoginPage>(null);
        }
    }
}
