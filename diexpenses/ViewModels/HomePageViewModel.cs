namespace diexpenses.ViewModels
{
    using Common;
    using diexpenses.Services;
    using diexpenses.Services.Database;
    using diexpenses.ViewModels.Base;
    using OxyPlot;
    using OxyPlot.Series;
    using System;
    using Windows.UI.Xaml.Navigation;

    public class HomePageViewModel: MenuBottomViewModelBase
    {
        private string name;
        private double totalAmount;
        private double monthIncomes;
        private double monthExpenses;
        private string balance;

        private PlotModel chartModel = new PlotModel();

        private IDbService dbService;

        public HomePageViewModel(IDbService dbService, INavigationService navigationService) : base(navigationService)
        {
            this.dbService = dbService;

            var today = DateTime.Today;
            name = Utils.GetLoggedUserName();
            totalAmount = dbService.SelectTotalAmount();
            monthIncomes = dbService.SelectMonthlAmount(false, today.Year, today.Month);
            monthExpenses = dbService.SelectMonthlAmount(true, today.Year, today.Month);
            SetChart();
            DoBalance();
        }

        public override void NavigateTo(NavigationEventArgs e)
        {
            base.NavigateTo(e);

            this.NavigationService.AppFrame = base.AppFrame;
        }

        private void SetChart()
        {            
            dynamic series = new PieSeries { StrokeThickness = 2.0, InsideLabelPosition = 0.8, AngleSpan = 360, StartAngle = 0 };

            series.Slices.Add(new PieSlice("Expenses", MonthExpenses) { IsExploded = false, Fill = OxyColors.PaleVioletRed });
            series.Slices.Add(new PieSlice("Incomes", MonthIncomes) { IsExploded = true, Fill = OxyColors.SpringGreen });

            ChartModel.Series.Add(series);
        }

        private void DoBalance()
        {
            if(monthIncomes > monthExpenses)
            {
                balance = "This month you are doing well";
            } else if (monthIncomes < monthExpenses)
            {
                balance = "Do not spend much!";
            } else
            {
                balance = "Spares nothing...";
            }
        }

        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                RaisePropertyChanged();
            }
        }

        public double TotalAmount
        {
            get { return this.totalAmount; }
            set
            {
                this.totalAmount = value;
                RaisePropertyChanged();
            }
        }

        public double MonthIncomes
        {
            get { return this.monthIncomes; }
            set
            {
                this.monthIncomes = value;
                RaisePropertyChanged();
            }
        }

        public double MonthExpenses
        {
            get { return this.monthExpenses; }
            set
            {
                this.monthExpenses = value;
                RaisePropertyChanged();
            }
        }

        public string Balance
        {
            get { return this.balance; }
            set
            {
                this.balance = value;
                RaisePropertyChanged();
            }
        }

        public PlotModel ChartModel
        {
            get { return this.chartModel; }
            set
            {
                this.chartModel = value;
                RaisePropertyChanged();
            }
        }
    }
}
