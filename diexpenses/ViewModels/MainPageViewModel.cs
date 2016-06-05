namespace diexpenses.ViewModels
{
    using Base;
    using Common;
    using Services;
    using System;
    using System.Diagnostics;
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
            var Settings = ApplicationData.Current.LocalSettings;
            var isLogged = Settings.Values[Constants.IS_LOGGED];
            if (isLogged == null || !Boolean.Parse(isLogged.ToString()))
            {
                Debug.WriteLine("User is NOT logged in the APP");

                navigationService.NavigateToLoginPage<Object>(null);
            }
            else
            {
                Debug.WriteLine("User is logged in the APP");
                // TODO Crear la HomePage
                //this.navigationService.NavigateToHomePage<Object>(null);
            }
        }
    }
}
