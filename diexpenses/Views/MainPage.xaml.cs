namespace diexpenses
{
    using ViewModels;
    using Views.Base;
    using Windows.UI.Xaml;

    public sealed partial class MainPage : ViewBase
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            MainPageViewModel.Redirect();
        }

    }
}
