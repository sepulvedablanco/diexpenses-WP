namespace diexpenses.ViewModels
{
    using common.Entities;
    using diexpenses.Services;
    using common.Services.Database;
    using diexpenses.ViewModels.Base;
    using Services.StorageService;
    using Windows.UI.Xaml.Navigation;

    public class MovementDetailsPageViewModel : MenuBottomViewModelBase
    {
        private static Movement movement;

        public MovementDetailsPageViewModel(INavigationService navigationService, IDbService dbService, IStorageService storageService) : base(navigationService, dbService, storageService)
        {
        }

        public override void NavigateTo(NavigationEventArgs e)
        {
            base.NavigateTo(e);

            this.NavigationService.AppFrame = base.AppFrame;
        }

        public static Movement Movement
        {
            get { return movement; }
            set
            {
                movement = value;
            }
        }
    }
}
