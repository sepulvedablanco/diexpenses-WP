using diexpenses.Services;
using diexpenses.ViewModels.Base;
using System.Diagnostics;
using Windows.UI.Xaml.Navigation;

namespace diexpenses.ViewModels
{
    public class BankAccountDetailsPageViewModel : ViewModelBase
    {
        private static DelegateCommand actionCommand;

        private IDialogService dialogService;
        private INavigationService navigationService;

        public BankAccountDetailsPageViewModel(IDialogService dialogService, INavigationService navigationService)
        {
            this.dialogService = dialogService;
            this.navigationService = navigationService;

            actionCommand = new DelegateCommand(ActionExecute, ActionCanExecute);
        }

        private void ActionExecute()
        {
            Debug.WriteLine("ActionExecute");
        }

        private bool ActionCanExecute()
        {
            Debug.WriteLine("ActionCanExecute");
            return true;
        }

        public override void NavigateTo(NavigationEventArgs e)
        {
            base.NavigateTo(e);

            this.navigationService.AppFrame = base.AppFrame;
        }
    }
}
