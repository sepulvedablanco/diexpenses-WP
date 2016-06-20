﻿namespace diexpenses.ViewModels
{
    using diexpenses.Entities;
    using diexpenses.Services;
    using diexpenses.ViewModels.Base;
    using Services.Database;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Windows.Input;
    using Views;
    using Windows.UI.Xaml.Navigation;

    public class BankAccountsListPageViewModel : MenuBottomViewModelBase
    {
        private ObservableCollection<BankAccount> items;

        private static DelegateCommand newBankAccountCommand;
        private static Base.DelegateCommandWithParameter<BankAccount> editBankAccountCommand;
        private static Base.DelegateCommandWithParameter<BankAccount> deleteBankAccountCommand;

        private IDbService dbService;
        private IDialogService dialogService;

        public BankAccountsListPageViewModel(IDbService dbService, INavigationService navigationService, IDialogService dialogService) : base(navigationService)
        {
            this.dbService = dbService;
            this.dialogService = dialogService;

            newBankAccountCommand = new DelegateCommand(NewBankAccountExecute, null);
            editBankAccountCommand = new Base.DelegateCommandWithParameter<BankAccount>(EditBankAccountExecute, null);
            deleteBankAccountCommand = new Base.DelegateCommandWithParameter<BankAccount>(DeleteBankAccountExecute, null);

            LoadBankAccounts();
        }

        private void LoadBankAccounts()
        {
            var bankAccountsList = this.dbService.SelectBankAccounts();
            Debug.WriteLine("Number of bank accounts retrieved: " + bankAccountsList.Count);
            Items = new ObservableCollection<BankAccount>(bankAccountsList);
        }

        public ICommand NewBankAccountCommand
        {
            get { return newBankAccountCommand; }
        }

        public ICommand EditBankAccountCommand
        {
            get { return editBankAccountCommand; }
        }

        public ICommand DeleteBankAccountCommand
        {
            get { return deleteBankAccountCommand; }
        }

        private void NewBankAccountExecute()
        {
            Debug.WriteLine("NewBankAccountExecute");

            NavigationService.NavigateTo(new BankAccountDetailsPage(new BankAccount()));
        }

        private void EditBankAccountExecute(BankAccount bankAccount)
        {
            Debug.WriteLine("EditBankAccountExecute");
            Debug.WriteLine("Bank account to edit: " + bankAccount.ToString());

            NavigationService.NavigateTo(new BankAccountDetailsPage(bankAccount));
        }

        private async void DeleteBankAccountExecute(BankAccount bankAccount)
        {
            Debug.WriteLine("DeleteBankAccountExecute");
            Debug.WriteLine("Bank account to delete: " + bankAccount.ToString());

            bool result = await dialogService.ShowConfirmMessage("Delete bank account", "Are you sure you want to delete the bank account " + bankAccount.Description, "I agree", "Delete", "Cancel");
            Debug.WriteLine("Delete bank account: " + result);
            if (result)
            {
                if (dbService.Delete<BankAccount>(bankAccount))
                {
                    LoadBankAccounts();
                }
            }
        }

        public ObservableCollection<BankAccount> Items
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
