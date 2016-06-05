﻿using diexpenses.Services;
using diexpenses.ViewModels.Base;
using System;
using System.Diagnostics;
using System.Windows.Input;
using Windows.UI.Xaml.Navigation;

namespace diexpenses.ViewModels
{
    public class SignupPageViewModel: ViewModelBase
    {
        private string name;
        private string username;

        public static Func<string> PasswordHandler { get; set; }
        public static Func<string> ConfirmPasswordHandler { get; set; }

        private static DelegateCommand createAccountCommand;
        private static DelegateCommand loginCommand;

        private IDialogService dialogService;
        private INavigationService navigationService;

        public SignupPageViewModel(IDialogService dialogService, INavigationService navigationService)
        {
            this.dialogService = dialogService;
            this.navigationService = navigationService;

            createAccountCommand = new DelegateCommand(CreateAccountExecute, CreateAccountCanExecute);
            loginCommand = new DelegateCommand(NavigateToLoginExecute, null);
        }
        
        public ICommand CreateAccountCommand
        {
            get { return createAccountCommand; }
        }

        public ICommand LoginCommand
        {
            get { return loginCommand; }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                loginCommand.RaiseCanExecuteChanged();
            }
        }

        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                loginCommand.RaiseCanExecuteChanged();
            }
        }

        private void CreateAccountExecute()
        {
            Name = this.name;
            Username = this.username;

            Debug.WriteLine("Name=" + Name + ". Username=" + Username + ". Password=" + PasswordHandler() + ". Confirm Password=" + ConfirmPasswordHandler());

            if(!PasswordHandler().Equals(ConfirmPasswordHandler()))
            {
                dialogService.ShowMessage("Passwords does not match!");
                return;
            }

            CreateUser();
        }

        private bool CreateAccountCanExecute()
        {
            Debug.WriteLine("Password=" + PasswordHandler() + ". Confirm password=" + ConfirmPasswordHandler());

            if (string.IsNullOrEmpty(this.name) || string.IsNullOrEmpty(this.username) || 
                string.IsNullOrEmpty(PasswordHandler()) || string.IsNullOrEmpty(ConfirmPasswordHandler())) { return false; }
            return true;
        }

        public override void NavigateTo(NavigationEventArgs e)
        {
            base.NavigateTo(e);

            this.navigationService.AppFrame = base.AppFrame;
        }

        private void NavigateToLoginExecute()
        {
            Debug.WriteLine("Navigating to Login view");
            this.navigationService.NavigateToLoginPage<Object>(null);
        }

        public void CreateUser()
        {

        }

        public static void OnPasswordChanged()
        {
            createAccountCommand.RaiseCanExecuteChanged();
        }

        public static void OnConfirmPasswordChanged()
        {
            createAccountCommand.RaiseCanExecuteChanged();
        }
    }
}
