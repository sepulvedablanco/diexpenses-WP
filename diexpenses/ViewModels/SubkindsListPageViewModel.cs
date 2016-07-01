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

    public class SubkindsListPageViewModel : MenuBottomViewModelBase
    {
        private static Kind kind;

        private ObservableCollection<Subkind> items;

        private static DelegateCommand newSubkindCommand;
        private static DelegateCommand editSubkindCommand;
        private static DelegateCommand deleteSubkindCommand;

        private IDialogService dialogService;

        public SubkindsListPageViewModel(IDbService dbService, INavigationService navigationService, IDialogService dialogService, IStorageService storageService) : base(navigationService, dbService, storageService)
        {
            this.dialogService = dialogService;

            newSubkindCommand = new DelegateCommand(NewSubkindExecute);
            editSubkindCommand = new DelegateCommand(EditSubkindExecute);
            deleteSubkindCommand = new DelegateCommand(DeleteSubkindExecute);

            LoadSubkinds();
        }

        private void LoadSubkinds()
        {
            if(Kind == null || Kind.Id == null)
            {
                Debug.WriteLine("Invalid kind. Subkind search won't be executed.");
                return;
            }
            var subkindsList = this.DbService.SelectSubkinds(Kind.Id.GetValueOrDefault());
            Debug.WriteLine("Number of subkinds retrieved: " + subkindsList.Count);
            Items = new ObservableCollection<Subkind>(subkindsList);
        }

        public ICommand NewSubkindCommand
        {
            get { return newSubkindCommand; }
        }

        public ICommand EditSubkindCommand
        {
            get { return editSubkindCommand; }
        }

        public ICommand DeleteSubkindCommand
        {
            get { return deleteSubkindCommand; }
        }

        private async void NewSubkindExecute()
        {
            Debug.WriteLine("NewSubkindExecute");
            string result = await dialogService.ShowMessage("New Subkind", "Introduce the new subkind", null, "Save", "Cancel");
            Debug.WriteLine("New subkind name: " + result);
            if(!string.IsNullOrEmpty(result))
            {
                Subkind subkind = new Subkind(kind.Id.GetValueOrDefault(), result);
                DbService.Upsert<Subkind>(subkind);
                LoadSubkinds();
            }
        }

        private async void EditSubkindExecute()
        {
            Debug.WriteLine("EditSubkindExecute");
            Subkind subkind = OpenMenuFlyoutAction.HoldedObject as Subkind;
            if (subkind == null)
            {
                Debug.WriteLine("Invalid subkind to edit...");
                return;
            }

            Debug.WriteLine("Subkind to edit: " + subkind.ToString());

            string result = await dialogService.ShowMessage("Edit Subkind", "Introduce the new subkind name", subkind.Description, "Save", "Cancel");
            Debug.WriteLine("Edited subkind name: " + result);
            if (!string.IsNullOrEmpty(result))
            {
                subkind.Description = result;
                DbService.Upsert<Subkind>(subkind);
                LoadSubkinds();
            }
        }

        private async void DeleteSubkindExecute()
        {
            Debug.WriteLine("DeleteSubkindExecute");
            Subkind subkind = OpenMenuFlyoutAction.HoldedObject as Subkind;
            if (subkind == null)
            {
                Debug.WriteLine("Invalid subkind to delete...");
                return;
            }

            Debug.WriteLine("Subkind to delete: " + subkind.ToString());

            bool result = await dialogService.ShowConfirmMessage("Delete subkind", "Are you sure you want to delete the subkind " + subkind.Description, "I agree", "Delete", "Cancel");
            Debug.WriteLine("Delete subkind: " + result);
            if (result)
            {
                if (DbService.Delete<Subkind>(subkind))
                {
                    LoadSubkinds();
                }
            }
        }

        public static Kind Kind
        {
            get { return kind; }
            set
            {
                kind = value;
            }
        }

        public ObservableCollection<Subkind> Items
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
