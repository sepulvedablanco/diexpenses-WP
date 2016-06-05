namespace diexpenses.Views
{
    using Base;
    using ViewModels;
    using Windows.UI.Xaml;

    public sealed partial class SignupPage : ViewBase
    {
        public SignupPage()
        {
            this.InitializeComponent();

            SignupPageViewModel.PasswordHandler = () => PasswordTextBox.Password;
            SignupPageViewModel.ConfirmPasswordHandler = () => ConfirmPasswordTextBox.Password;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            SignupPageViewModel.OnPasswordChanged();
        }

        private void ConfirmPasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            SignupPageViewModel.OnConfirmPasswordChanged();
        }
    }
}
