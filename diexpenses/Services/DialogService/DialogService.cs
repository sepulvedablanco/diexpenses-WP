namespace diexpenses.Services
{
    using System;
    using Windows.UI.Popups;

    public class DialogService : IDialogService
    {
        public async void ShowMessage(string message)
        {
            MessageDialog dialog = new MessageDialog(message);
            await dialog.ShowAsync();
        }
    }
}
