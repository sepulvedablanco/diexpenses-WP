using diexpenses.Services;
using diexpenses.ViewModels.Base;
using System.Diagnostics;
using Windows.UI.Xaml.Navigation;

namespace diexpenses.ViewModels
{
    public class NewMovementPageViewModel : ViewModelBase
    {
        private static DelegateCommand saveCommand;

        private IDialogService dialogService;
        private INavigationService navigationService;

        public NewMovementPageViewModel(IDialogService dialogService, INavigationService navigationService)
        {
            this.dialogService = dialogService;
            this.navigationService = navigationService;

            saveCommand = new DelegateCommand(SaveExecute, SaveCanExecute);
        }

        private void SaveExecute()
        {
            Debug.WriteLine("SaveExecute");
        }

        private bool SaveCanExecute()
        {
            Debug.WriteLine("SaveCanExecute");
            return true;
        }

        public override void NavigateTo(NavigationEventArgs e)
        {
            base.NavigateTo(e);

            this.navigationService.AppFrame = base.AppFrame;
        }
    }
}
