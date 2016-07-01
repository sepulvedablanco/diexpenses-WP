﻿namespace diexpenses.ViewModels.Base
{
    using common.Common;
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
    using common.Services.Database;
    using NotificationsExtensions;
    using System.Globalization;
    using Services.StorageService;

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
            var today = DateTime.Today;
            var monthIncomes = dbService.SelectMonthlAmount(false, today.Year, today.Month);
            var monthExpenses = dbService.SelectMonthlAmount(true, today.Year, today.Month);
            var totalAmount = dbService.SelectTotalAmount();
            var monthIncomesFormatted = String.Format(CultureInfo.InvariantCulture, Constants.AMOUNT_FORMAT, monthIncomes) + "€";
            var monthExpensesFormatted = String.Format(CultureInfo.InvariantCulture, Constants.AMOUNT_FORMAT, monthExpenses) + "€";
            var totalAmountFormatted = String.Format(CultureInfo.InvariantCulture, Constants.AMOUNT_FORMAT, totalAmount) + "€";

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
                        // TileSmall => only diexpenses icon...
                        TileMedium = new TileBinding()
                        {
                            Content = new TileBindingContentAdaptive()
                            {
                                Children = {
                                    new AdaptiveText()
                                    {
                                        Text = "diexpenses",
                                        HintStyle = AdaptiveTextStyle.Title,
                                        HintAlign = AdaptiveTextAlign.Center
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = "Expenses: " + monthExpensesFormatted,
                                        HintStyle = AdaptiveTextStyle.Body,
                                        HintAlign = AdaptiveTextAlign.Left
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = "Incomes: " + monthIncomesFormatted,
                                        HintStyle = AdaptiveTextStyle.Body,
                                        HintAlign = AdaptiveTextAlign.Left
                                    }

                                },
                                PeekImage = new TilePeekImage
                                {
                                    AlternateText = "ou yeah",
                                    Source = "ms-appdata:///local/tiles/tileImage.svg",
                                    HintOverlay = 5,
                                    HintCrop = TilePeekImageCrop.None
                                }
                            }
                        },
                        TileWide = new TileBinding()
                        {
                            Content = new TileBindingContentAdaptive()
                            {
                                Children = {
                                    new AdaptiveText()
                                    {
                                        Text = "diexpenses",
                                        HintStyle = AdaptiveTextStyle.Title,
                                        HintAlign = AdaptiveTextAlign.Center
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = "Expenses: " + monthExpensesFormatted,
                                        HintStyle = AdaptiveTextStyle.Body,
                                        HintAlign = AdaptiveTextAlign.Left
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = "Incomes: " + monthIncomesFormatted,
                                        HintStyle = AdaptiveTextStyle.Body,
                                        HintAlign = AdaptiveTextAlign.Left
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = "Total amount: " + totalAmountFormatted,
                                        HintStyle = AdaptiveTextStyle.Body,
                                        HintAlign = AdaptiveTextAlign.Left
                                    }
                                },
                                PeekImage = new TilePeekImage
                                {
                                    AlternateText = "ou yeah",
                                    //Source = "ms-appdata:///local/tiles/tileImage.svg",
                                    Source = "ms-appdata:///local/tiles/home.png",
                                    HintOverlay = 5,
                                    HintCrop = TilePeekImageCrop.Circle
                                    //HintCrop = TilePeekImageCrop.None
                                }
                            }
                        },
                        TileLarge = new TileBinding()
                        {
                            Content = new TileBindingContentAdaptive()
                            {
                                Children = {
                                    new AdaptiveText()
                                    {
                                        Text = "diexpenses",
                                        HintStyle = AdaptiveTextStyle.Title,
                                        HintAlign = AdaptiveTextAlign.Center
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = "Expenses: " + monthExpensesFormatted,
                                        HintStyle = AdaptiveTextStyle.Body,
                                        HintAlign = AdaptiveTextAlign.Left
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = "Incomes: " + monthIncomesFormatted,
                                        HintStyle = AdaptiveTextStyle.Body,
                                        HintAlign = AdaptiveTextAlign.Left
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = "Total amount: " + totalAmountFormatted,
                                        HintStyle = AdaptiveTextStyle.Body,
                                        HintAlign = AdaptiveTextAlign.Left
                                    }
                                },
                                PeekImage = new TilePeekImage
                                {
                                    AlternateText = "ou yeah",
                                    Source = "ms-appdata:///local/tiles/tileImage.svg",
                                    HintOverlay = 5,
                                    HintCrop = TilePeekImageCrop.None
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

            Common.Utils.DeleteUserDataInMemory();

            navigationService.NavigateTo<LoginPage>(null);
        }
    }
}
