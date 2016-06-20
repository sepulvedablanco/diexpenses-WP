namespace diexpenses.ViewModels
{
    using diexpenses.Entities;
    using diexpenses.Services;
    using diexpenses.ViewModels.Base;
    using Services.Database;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Windows.Input;
    using Views;

    public class KindsListPageViewModel : MenuBottomViewModelBase
    {
        private ObservableCollection<Kind> items;

        private static DelegateCommand newKindCommand;
        private static Base.DelegateCommandWithParameter<Kind> kindSelectedCommand;
        private static Base.DelegateCommandWithParameter<Kind> editKindCommand;
        private static Base.DelegateCommandWithParameter<Kind> deleteKindCommand;

        private IDbService dbService;
        private IDialogService dialogService;

        public KindsListPageViewModel(IDbService dbService, INavigationService navigationService, IDialogService dialogService) : base(navigationService)
        {
            this.dbService = dbService;
            this.dialogService = dialogService;

            newKindCommand = new DelegateCommand(NewKindExecute, null);
            kindSelectedCommand = new Base.DelegateCommandWithParameter<Kind>(KindSelectedExecute, null);
            editKindCommand = new Base.DelegateCommandWithParameter<Kind>(EditKindExecute, null);
            deleteKindCommand = new Base.DelegateCommandWithParameter<Kind>(DeleteKindExecute, null);

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

        public ICommand KindSelectedCommand
        {
            get { return kindSelectedCommand; }
        }

        public ICommand EditKindCommand
        {
            get { return editKindCommand; }
        }

        public ICommand DeleteKindCommand
        {
            get { return deleteKindCommand; }
        }

        private async void NewKindExecute()
        {
            Debug.WriteLine("NewKindExecute");
            string result = await dialogService.ShowMessage("New Kind", "Introduce the new kind", null, "Save", "Cancel");
            Debug.WriteLine("New kind name: " + result);
            if(!string.IsNullOrEmpty(result))
            {
                Kind kind = new Kind(result);
                dbService.Upsert<Kind>(kind);
                LoadKinds();
            }
        }

        private void KindSelectedExecute(Kind kind)
        {
            Debug.WriteLine("KindSelectedExecute");
            Debug.WriteLine("Kind selected: " + kind.ToString());

            NavigationService.NavigateTo(new SubkindsListPage(kind));
        }

        private async void EditKindExecute(Kind kind)
        {
            Debug.WriteLine("EditKindExecute");
            Debug.WriteLine("Kind to edit: "  + kind.ToString());

            string result = await dialogService.ShowMessage("Edit Kind", "Introduce the new kind name", kind.Description, "Save", "Cancel");
            Debug.WriteLine("Edited kind name: " + result);
            if (!string.IsNullOrEmpty(result))
            {
                kind.Description = result;
                dbService.Upsert<Kind>(kind);
                LoadKinds();
            }
        }

        private async void DeleteKindExecute(Kind kind)
        {
            Debug.WriteLine("DeleteKindExecute");
            Debug.WriteLine("Kind to delete: " + kind.ToString());

            bool result = await dialogService.ShowConfirmMessage("Delete kind", "Are you sure you want to delete the kind " + kind.Description, "I agree", "Delete", "Cancel");
            Debug.WriteLine("Delete kind: " + result);
            if (result)
            {
                if (dbService.Delete<Kind>(kind))
                {
                    LoadKinds();
                }
            }
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
