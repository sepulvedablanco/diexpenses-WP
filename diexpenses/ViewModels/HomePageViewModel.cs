using diexpenses.Services;
using diexpenses.ViewModels.Base;
using Windows.UI.Xaml.Navigation;

namespace diexpenses.ViewModels
{
    public class HomePageViewModel: MenuBottomViewModelBase
    {
        public HomePageViewModel(INavigationService navigationService) : base(navigationService)
        {

        }

        public override void NavigateTo(NavigationEventArgs e)
        {
            base.NavigateTo(e);

            this.NavigationService.AppFrame = base.AppFrame;
        }
    }
}
