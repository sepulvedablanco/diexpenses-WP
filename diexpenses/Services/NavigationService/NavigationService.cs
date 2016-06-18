using diexpenses.Views;
using Windows.UI.Xaml.Controls;

namespace diexpenses.Services
{
    public class NavigationService : INavigationService
    {
        public Frame AppFrame { private get; set; }

        public void GoBack()
        {
            if (AppFrame != null && AppFrame.CanGoBack == true)
            {
                AppFrame.GoBack();
            }
        }

        public void NavigateTo<T>(T parameters)
        {
            if (AppFrame != null)
            {
                AppFrame.Navigate(typeof(T), parameters);
            }
        }

    }
}
