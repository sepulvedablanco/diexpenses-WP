namespace diexpenses.ViewModels
{
    using Common;
    using common.Entities;
    using diexpenses.Services;
    using diexpenses.ViewModels.Base;
    using common.Services.Database;
    using Services.StorageService;
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows.Input;
    using Views;
    using Windows.UI.Xaml.Navigation;

    public class MovementsListPageViewModel : MenuBottomViewModelBase
    {
        private ObservableCollection<int> years;
        private ObservableCollection<String> months;
        private ObservableCollection<Movement> items;
        private int selectedYear;
        private int selectedMonth;

        private static DelegateCommand newMovementCommand;
        private static DelegateCommand deleteMovementCommand;
        private static Base.DelegateCommandWithParameter<Movement> movementSelectedCommand;

        private static DelegateCommand yearChangedCommand;
        private static DelegateCommand monthChangedCommand;

        private IDialogService dialogService;

        public MovementsListPageViewModel(IDbService dbService, INavigationService navigationService, IDialogService dialogService, IStorageService storageService) : base(navigationService, dbService, storageService)
        {
            this.dialogService = dialogService;

            newMovementCommand = new DelegateCommand(NewMovementExecute);
            deleteMovementCommand = new DelegateCommand(DeleteMovementExecute);
            movementSelectedCommand = new Base.DelegateCommandWithParameter<Movement>(MovementSelectedExecute);

            yearChangedCommand = new DelegateCommand(YearChangedExecute);
            monthChangedCommand = new DelegateCommand(MonthChangedExecute);

            LoadYears();
            LoadMonths();

            DateTime today = DateTime.Today;
            SelectedYear = today.Year;
            SelectedMonth = today.Month - 1;
            LoadMovements();
        }

        private void LoadYears()
        {
            var today = DateTime.Today;
            years = new ObservableCollection<int>();
            for(int i = 2010; i <= today.Year; i++)
            {
                years.Add(i);
            }
            SelectedYear = today.Year;
        }

        private void LoadMonths()
        {
            months = new ObservableCollection<string>();
            for (int i = 1; i <= 12; i++)
            {
                months.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i));
            }
        }

        private void LoadMovements()
        {
            var month = SelectedMonth + 1;
            Debug.WriteLine("Year=" + SelectedYear + ", Month=" + month);
            var movementsList = this.DbService.SelectMonthlyMovements(SelectedYear, month);
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

        public ICommand YearChangedCommand
        {
            get { return yearChangedCommand; }
        }

        public ICommand MonthChangedCommand
        {
            get { return monthChangedCommand; }
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

        private void YearChangedExecute()
        {
            Debug.WriteLine("YearChangedExecute");
            LoadMovements();
        }

        private void MonthChangedExecute()
        {
            Debug.WriteLine("MonthChangedExecute");
            LoadMovements();
        }

        public ObservableCollection<int> Years
        {
            get { return this.years; }
            set
            {
                this.years = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<string> Months
        {
            get { return this.months; }
            set
            {
                this.months = value;
                RaisePropertyChanged();
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

        public int SelectedYear
        {
            get { return this.selectedYear; }
            set
            {
                this.selectedYear = value;
                RaisePropertyChanged();
            }
        }

        public int SelectedMonth
        {
            get { return this.selectedMonth; }
            set
            {
                this.selectedMonth = value;
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
