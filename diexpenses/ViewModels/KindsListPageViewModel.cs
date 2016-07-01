namespace diexpenses.ViewModels
{
    using Common;
    using common.Entities;
    using diexpenses.Services;
    using diexpenses.ViewModels.Base;
    using common.Services.Database;
    using Services.StorageService;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Windows.Input;
    using Views;

    public class KindsListPageViewModel : MenuBottomViewModelBase
    {
        private ObservableCollection<Kind> items;

        private static DelegateCommand newKindCommand;
        private static DelegateCommand editKindCommand;
        private static DelegateCommand deleteKindCommand;
        private static Base.DelegateCommandWithParameter<Kind> kindSelectedCommand;

        private IDialogService dialogService;

        public KindsListPageViewModel(IDbService dbService, INavigationService navigationService, IDialogService dialogService, IStorageService storageService) : base(navigationService, dbService, storageService)
        {
            this.dialogService = dialogService;

            newKindCommand = new DelegateCommand(NewKindExecute);
            editKindCommand = new DelegateCommand(EditKindExecute);
            deleteKindCommand = new DelegateCommand(DeleteKindExecute);
            kindSelectedCommand = new Base.DelegateCommandWithParameter<Kind>(KindSelectedExecute);

            LoadKinds();
        }

        private void LoadKinds()
        {
            var kindsList = this.DbService.SelectKinds();
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

        public ICommand DeleteKindCommand
        {
            get { return deleteKindCommand; }
        }

        public ICommand KindSelectedCommand
        {
            get { return kindSelectedCommand; }
        }

        private async void NewKindExecute()
        {
            Debug.WriteLine("NewKindExecute");
            string result = await dialogService.ShowMessage("New Kind", "Introduce the new kind", null, "Save", "Cancel");
            Debug.WriteLine("New kind name: " + result);
            if(!string.IsNullOrEmpty(result))
            {
                Kind kind = new Kind(result);
                DbService.Upsert<Kind>(kind);
                LoadKinds();
            }
        }

        private async void EditKindExecute()
        {
            Debug.WriteLine("EditKindExecute");
            Kind kind = OpenMenuFlyoutAction.HoldedObject as Kind;
            if(kind == null)
            {
                Debug.WriteLine("Invalid kind to edit...");
                return;
            }

            Debug.WriteLine("Kind to edit: "  + kind.ToString());

            string result = await dialogService.ShowMessage("Edit Kind", "Introduce the new kind name", kind.Description, "Save", "Cancel");
            Debug.WriteLine("Edited kind name: " + result);
            if (!string.IsNullOrEmpty(result))
            {
                kind.Description = result;
                DbService.Upsert<Kind>(kind);
                LoadKinds();
            }
        }

        private async void DeleteKindExecute()
        {
            Debug.WriteLine("DeleteKindExecute");
            Kind kind = OpenMenuFlyoutAction.HoldedObject as Kind;
            if (kind == null)
            {
                Debug.WriteLine("Invalid kind to delete...");
                return;
            }

            Debug.WriteLine("Kind to delete: " + kind.ToString());

            bool result = await dialogService.ShowConfirmMessage("Delete kind", "Are you sure you want to delete the kind " + kind.Description, "I agree", "Delete", "Cancel");
            Debug.WriteLine("Delete kind: " + result);
            if (result)
            {
                if (DbService.Delete<Kind>(kind))
                {
                    LoadKinds();
                }
            }
        }

        private void KindSelectedExecute(Kind kind)
        {
            Debug.WriteLine("KindSelectedExecute");
            Debug.WriteLine("Kind selected: " + kind.ToString());

            NavigationService.NavigateTo(new SubkindsListPage(kind));
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
