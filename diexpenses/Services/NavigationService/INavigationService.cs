using Windows.UI.Xaml.Controls;

namespace diexpenses.Services
{
    public interface INavigationService
    {
        Frame AppFrame { set; }

        void GoBack();

        void NavigateToLoginPage<T>(T parameters);

        void NavigateToSignupPage<T>(T parameters);

        void NavigateToHomePage<T>(T parameters);

        void NavigateToKindsListPage<T>(T parameters);

    }
}
