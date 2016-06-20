namespace diexpenses.ViewModels
{
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
        private static Base.DelegateCommandWithParameter<Movement> deleteMovementCommand;

        private IDbService dbService;
        private IDialogService dialogService;

        public MovementsListPageViewModel(IDbService dbService, INavigationService navigationService, IDialogService dialogService) : base(navigationService)
        {
            this.dbService = dbService;
            this.dialogService = dialogService;

            newMovementCommand = new DelegateCommand(NewMovementExecute, null);
            deleteMovementCommand = new Base.DelegateCommandWithParameter<Movement>(DeleteMovementExecute, null);

            LoadMovements();
        }

        private void LoadMovements()
        {
            DateTime today = new DateTime();
            var movementsList = this.dbService.SelectMonthlyMovements(today.Year, today.Month);
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

        private void NewMovementExecute()
        {
            Debug.WriteLine("NewMovementExecute");

            NavigationService.NavigateTo<NewMovementPage>(null);
        }

        private async void DeleteMovementExecute(Movement movement)
        {
            Debug.WriteLine("DeleteMovementExecute");
            Debug.WriteLine("Movement to delete: " + movement.ToString());

            bool result = await dialogService.ShowConfirmMessage("Delete movement", "Are you sure you want to delete the movement " + movement.Concept, "I agree", "Delete", "Cancel");
            Debug.WriteLine("Delete movement: " + result);
            if (result)
            {
                if (dbService.Delete<Movement>(movement))
                {
                    LoadMovements();
                }
            }
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
