namespace diexpenses.ViewModels
{
    using diexpenses.Entities;
    using diexpenses.Services;
    using diexpenses.ViewModels.Base;
    using Services.Database;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Windows.Input;
    using Windows.UI.Xaml.Navigation;

    public class KindsListPageViewModel : MenuBottomViewModelBase
    {
        private ObservableCollection<Kind> items;

        private static DelegateCommand newKindCommand;
        private static DelegateCommand editKindCommand;

        private IDbService dbService;
        private IDialogService dialogService;

        public KindsListPageViewModel(IDbService dbService, INavigationService navigationService, IDialogService dialogService) : base(navigationService)
        {
            this.dbService = dbService;
            this.dialogService = dialogService;

            newKindCommand = new DelegateCommand(NewKindExecute, null);
            editKindCommand = new DelegateCommand(EditKindExecute, null);

            LoadKinds();
        }

        private void LoadKinds()
        {
            var kindsList = this.dbService.SelectKinds();
            Debug.WriteLine("Number of kinds retrieved: " + kindsList.Count);
            Items = new ObservableCollection<Kind>(kindsList);
        }

        public ICommand NewKindCommand
        {
            get { return newKindCommand; }
        }

        public ICommand EditKindCommand
        {
            get { return editKindCommand; }
        }

        private async void NewKindExecute()
        {
            Debug.WriteLine("NewKindExecute");
            string result = await dialogService.ShowMessage("New Kind", "Introduce the new kind", null, "Save", "Cancel");
            Debug.WriteLine("New kind name: " + result);
            if(!string.IsNullOrEmpty(result))
            {
                Kind kind = new Kind(result);
                dbService.UpsertKind(kind);
                LoadKinds();
            }
        }

        private void EditKindExecute()
        {
            Debug.WriteLine("Edit kind");
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

        /*
        public override void NavigateTo(NavigationEventArgs e)
        {
            base.NavigateTo(e);

            this.NavigationService.AppFrame = base.AppFrame;
        }
        */
    }
}
