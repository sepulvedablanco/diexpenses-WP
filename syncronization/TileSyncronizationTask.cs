namespace syncronization
{
    using common.Common;
    using common.Services.Database;
    using NotificationsExtensions;
    using NotificationsExtensions.Tiles;
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using Windows.ApplicationModel.Background;
    using Windows.UI.Notifications;
    using Windows.UI.StartScreen;

    public sealed class TileSyncronizationTask : IBackgroundTask
    {

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            if (!Utils.UserIsLogged())
            {
                return;
            }

            DbService dbService = new DbService();

            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            await RefreshTile(dbService);

            deferral.Complete();
        }

        private async Task RefreshTile(DbService dbService)
        {
            var today = DateTime.Today;
            var monthIncomes = dbService.SelectMonthlAmount(false, today.Year, today.Month);
            var monthExpenses = dbService.SelectMonthlAmount(true, today.Year, today.Month);
            var totalAmount = dbService.SelectTotalAmount();
            var monthIncomesFormatted = String.Format(CultureInfo.InvariantCulture, Constants.AMOUNT_FORMAT, monthIncomes) + "€";
            var monthExpensesFormatted = String.Format(CultureInfo.InvariantCulture, Constants.AMOUNT_FORMAT, monthExpenses) + "€";
            var totalAmountFormatted = String.Format(CultureInfo.InvariantCulture, Constants.AMOUNT_FORMAT, totalAmount) + "€";

            string tileId = Utils.GetTileId();
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
            }
        }
    }
}
