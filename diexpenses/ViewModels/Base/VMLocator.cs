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
        
    }
}
