namespace diexpenses.Services
{
    using System.Threading.Tasks;

    public interface IDialogService
    {
        void ShowAlert(string message);

        Task<string> ShowMessage(string title, string message, string tbText, string primaryButtonText, string secondaryButtonText);

        Task<bool> ShowConfirmMessage(string title, string message, string cbText, string primaryButtonText, string secondaryButtonText);

    }
}
