namespace diexpenses
{
    using ViewModels;
    using Windows.UI.Xaml;

    public sealed partial class MainPage
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
