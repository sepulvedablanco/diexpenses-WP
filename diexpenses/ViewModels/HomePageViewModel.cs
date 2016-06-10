using diexpenses.Services;
using diexpenses.ViewModels.Base;
using System;
using System.Diagnostics;
using System.Windows.Input;
using Windows.UI.Xaml.Navigation;

namespace diexpenses.ViewModels
{
    public class HomePageViewModel: ViewModelBase
    {
        private INavigationService navigationService;

        public HomePageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }
        
        public override void NavigateTo(NavigationEventArgs e)
        {
            base.NavigateTo(e);

            this.navigationService.AppFrame = base.AppFrame;
        }
    }
}
