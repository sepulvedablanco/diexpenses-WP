namespace diexpenses.ViewModels
{
    using diexpenses.Entities;
    using diexpenses.Services;
    using diexpenses.ViewModels.Base;
    using Services.Database;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using Windows.UI.Xaml.Navigation;

    public class KindsListPageViewModel : MenuBottomViewModelBase
    {
        private ObservableCollection<Kind> items;

        private IDbService dbService;

        public KindsListPageViewModel(IDbService dbService, INavigationService navigationService) : base(navigationService)
        {
            this.dbService = dbService;

            var kindsList = this.dbService.SelectKinds();
            Debug.WriteLine("Number of kinds retrieved: " + kindsList.Count);
            Items = new ObservableCollection<Kind>(kindsList);
        }

        public ObservableCollection<Kind> Items
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
