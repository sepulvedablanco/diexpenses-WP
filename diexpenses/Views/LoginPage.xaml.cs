namespace diexpenses.Views
{
    using ViewModels;
    using Windows.UI.Xaml;

    public sealed partial class LoginPage
    {
        public LoginPage()
        {
            this.InitializeComponent();

            LoginPageViewModel.PasswordHandler = () => PasswordTextBox.Password;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            LoginPageViewModel.OnPasswordChanged();
        }
    }
}
