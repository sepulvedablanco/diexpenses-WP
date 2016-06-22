using diexpenses.Entities;
using diexpenses.Services;
using diexpenses.ViewModels.Base;
using Windows.UI.Xaml.Navigation;

namespace diexpenses.ViewModels
{
    public class MovementDetailsPageViewModel : MenuBottomViewModelBase
    {
        private static Movement movement;

        public MovementDetailsPageViewModel(INavigationService navigationService) : base(navigationService)
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
