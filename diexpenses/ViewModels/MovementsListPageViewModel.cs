namespace diexpenses.ViewModels
{
    using Common;
    using diexpenses.Entities;
    using diexpenses.Services;
    using diexpenses.ViewModels.Base;
    using Services.Database;
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Windows.Input;
    using Views;
    using Windows.UI.Xaml.Navigation;

    public class MovementsListPageViewModel : MenuBottomViewModelBase
    {
        private ObservableCollection<Movement> items;

        private static DelegateCommand newMovementCommand;
        private static DelegateCommand deleteMovementCommand;
        private static Base.DelegateCommandWithParameter<Movement> movementSelectedCommand;

        private IDialogService dialogService;

        public MovementsListPageViewModel(IDbService dbService, INavigationService navigationService, IDialogService dialogService) : base(navigationService, dbService)
        {
            this.dialogService = dialogService;

            newMovementCommand = new DelegateCommand(NewMovementExecute);
            deleteMovementCommand = new DelegateCommand(DeleteMovementExecute);
            movementSelectedCommand = new Base.DelegateCommandWithParameter<Movement>(MovementSelectedExecute);

            LoadMovements();
        }

        private void LoadMovements()
        {
            DateTime today = DateTime.Today;
            Debug.WriteLine("Year=" + today.Year + ", Month=" + today.Month);
            var movementsList = this.DbService.SelectMonthlyMovements(today.Year, today.Month);
            Debug.WriteLine("Number of movements retrieved: " + movementsList.Count);
            Items = new ObservableCollection<Movement>(movementsList);
        }

        public ICommand NewMovementCommand
        {
            get { return newMovementCommand; }
        }

        public ICommand DeleteMovementCommand
        {
            get { return deleteMovementCommand; }
        }

        public ICommand MovementSelectedCommand
        {
            get { return movementSelectedCommand; }
        }

        private void NewMovementExecute()
        {
            Debug.WriteLine("NewMovementExecute");

            NavigationService.NavigateTo<NewMovementPage>(null);
        }

        private async void DeleteMovementExecute()
        {
            Debug.WriteLine("DeleteMovementExecute");
            Movement movement = OpenMenuFlyoutAction.HoldedObject as Movement;
            if (movement == null)
            {
                Debug.WriteLine("Invalid movement to delete...");
                return;
            }

            Debug.WriteLine("Movement to delete: " + movement.ToString());
            bool result = await dialogService.ShowConfirmMessage("Delete movement", "Are you sure you want to delete the movement " + movement.Concept, "I agree", "Delete", "Cancel");
            Debug.WriteLine("Delete movement: " + result);
            if (result)
            {
                if (DbService.Delete<Movement>(movement))
                {
                    LoadMovements();
                }
            }
        }

        private void MovementSelectedExecute(Movement movement)
        {
            Debug.WriteLine("MovementSelectedExecute");
            Debug.WriteLine("Movement selected: " + movement.ToString());

            NavigationService.NavigateTo(new MovementDetailsPage(movement));
        }

        public ObservableCollection<Movement> Items
        {
            get { return this.items; }
            set
            {
                this.items = value;
                RaisePropertyChanged();
            }
        }

        public override void NavigateTo(NavigationEventArgs e)
        {
            base.NavigateTo(e);

            this.NavigationService.AppFrame = base.AppFrame;
        }
    }
}
