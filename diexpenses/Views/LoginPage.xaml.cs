namespace diexpenses.Views
{
    using Base;
    using ViewModels;
    using Windows.UI.Xaml;

    public sealed partial class LoginPage : ViewBase
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
