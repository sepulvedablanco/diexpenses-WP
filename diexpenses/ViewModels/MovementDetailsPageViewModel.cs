using diexpenses.Entities;
using diexpenses.Services;
using diexpenses.ViewModels.Base;
using Windows.UI.Xaml.Navigation;

namespace diexpenses.ViewModels
{
    public class MovementDetailsPageViewModel : ViewModelBase
    {
        private static Movement movement;

        private IDialogService dialogService;
        private INavigationService navigationService;

        public MovementDetailsPageViewModel(IDialogService dialogService, INavigationService navigationService)
        {
            this.dialogService = dialogService;
            this.navigationService = navigationService;
        }

        public override void NavigateTo(NavigationEventArgs e)
        {
            base.NavigateTo(e);

            this.navigationService.AppFrame = base.AppFrame;
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
