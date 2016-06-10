namespace diexpenses.ViewModels.Base
{
    using Autofac;
    using Services;

    public class VMLocator
    {
        private IContainer container;

        public VMLocator()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<DialogService>().As<IDialogService>();
            builder.RegisterType<NavigationService>().As<INavigationService>();

            builder.RegisterType<MainPageViewModel>();
            builder.RegisterType<LoginPageViewModel>();
            builder.RegisterType<SignupPageViewModel>();
            builder.RegisterType<HomePageViewModel>();
            builder.RegisterType<BankAccountDetailsPageViewModel>();
            builder.RegisterType<NewMovementPageViewModel>();
            builder.RegisterType<MovementDetailsPageViewModel>();

            this.container = builder.Build();
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

        public BankAccountDetailsPageViewModel BankAccountDetailsPageVM
        {
            get { return this.container.Resolve<BankAccountDetailsPageViewModel>(); }
        }

        public NewMovementPageViewModel NewMovementPageVM
        {
            get { return this.container.Resolve<NewMovementPageViewModel>(); }
        }

        public MovementDetailsPageViewModel MovementDetailsPageVM
        {
            get { return this.container.Resolve<MovementDetailsPageViewModel>(); }
        }
    }
}
