namespace diexpenses.ViewModels
{
    using Base;
    using common.Common;
    using Services;
    using System;
    using System.Diagnostics;
    using Views;
    using Windows.Storage;
    using Windows.UI.Xaml.Navigation;

    public class MainPageViewModel : ViewModelBase
    {

        private static INavigationService navigationService;

        public MainPageViewModel(INavigationService navigationService)
        {
            MainPageViewModel.navigationService = navigationService;
        }

        public override void NavigateTo(NavigationEventArgs e)
        {
            base.NavigateTo(e);

            navigationService.AppFrame = base.AppFrame;
        }
        
        public static void Redirect()
        {
            if (Utils.UserIsLogged())
            {
                Debug.WriteLine("User is logged in the APP");
                navigationService.NavigateTo<HomePage>(null);
            }
            else
            {
                Debug.WriteLine("User is NOT logged in the APP");
                navigationService.NavigateTo<LoginPage>(null);
            }
        }
    }
}
