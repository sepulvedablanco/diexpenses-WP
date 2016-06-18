using Windows.UI.Xaml.Controls;

namespace diexpenses.Services
{
    public interface INavigationService
    {
        Frame AppFrame { set; }

        void GoBack();

        void NavigateTo<T>(T parameters);

    }
}
