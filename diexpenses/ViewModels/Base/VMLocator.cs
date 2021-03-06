﻿namespace diexpenses.ViewModels.Base
{
    using Autofac;
    using Services;
    using common.Services.Database;
    using common.Services.DiexpensesAPI;
    using common.Services.NetworkService;
    using Services.GpsService;
    using Services.StorageService;

    public class VMLocator
    {
        private IContainer container;

        public VMLocator()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<DialogService>().As<IDialogService>();
            builder.RegisterType<NavigationService>().As<INavigationService>();
            builder.RegisterType<DbService>().As<IDbService>();
            builder.RegisterType<NetworkService>().As<INetworkService>().SingleInstance();
            builder.RegisterType<ApiService>().As<IApiService>();
            builder.RegisterType<GpsService>().As<IGpsService>();
            builder.RegisterType<StorageService>().As<IStorageService>();

            builder.RegisterType<MainPageViewModel>();
            builder.RegisterType<LoginPageViewModel>();
            builder.RegisterType<SignupPageViewModel>();
            builder.RegisterType<HomePageViewModel>();
            builder.RegisterType<KindsListPageViewModel>();
            builder.RegisterType<SubkindsListPageViewModel>();
            builder.RegisterType<BankAccountsListPageViewModel>();
            builder.RegisterType<BankAccountDetailsPageViewModel>();
            builder.RegisterType<MovementsListPageViewModel>();
            builder.RegisterType<NewMovementPageViewModel>();
            builder.RegisterType<MovementDetailsPageViewModel>();
            builder.RegisterType<MenuBottomViewModelBase>();

            this.container = builder.Build();
        }

        public T Resolve<T>()
        {
            return this.container.Resolve<T>();
        }

        public MainPageViewModel MainPageVM
        {
            get { return this.container.Resolve<MainPageViewModel>(); }
        }
        
        public LoginPageViewModel LoginPageVM
        {
            get { return this.container.Resolve<LoginPageViewModel>(); }
        }
        
        public SignupPageViewModel SignupPageVM
        {
            get { return this.container.Resolve<SignupPageViewModel>();  }
        }

        public HomePageViewModel HomePageVM
        {
            get { return this.container.Resolve<HomePageViewModel>(); }
        }

        public KindsListPageViewModel KindsListPageVM
        {
            get { return this.container.Resolve<KindsListPageViewModel>(); }
        }

        public SubkindsListPageViewModel SubkindsListPageVM
        {
            get { return this.container.Resolve<SubkindsListPageViewModel>(); }
        }

        public BankAccountsListPageViewModel BankAccountsListPageVM
        {
            get { return this.container.Resolve<BankAccountsListPageViewModel>(); }
        }

        public BankAccountDetailsPageViewModel BankAccountDetailsPageVM
        {
            get { return this.container.Resolve<BankAccountDetailsPageViewModel>(); }
        }

        public MovementsListPageViewModel MovementsListPageVM
        {
            get { return this.container.Resolve<MovementsListPageViewModel>(); }
        }

        public NewMovementPageViewModel NewMovementPageVM
        {
            get { return this.container.Resolve<NewMovementPageViewModel>(); }
        }

        public MovementDetailsPageViewModel MovementDetailsPageVM
        {
            get { return this.container.Resolve<MovementDetailsPageViewModel>(); }
        }

        public MenuBottomViewModelBase MenuBottomVM
        {
            get { return this.container.Resolve<MenuBottomViewModelBase>(); }
        }
    }
}
