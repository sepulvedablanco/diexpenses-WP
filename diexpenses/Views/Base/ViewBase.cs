namespace diexpenses.Views.Base
{
    using ViewModels.Base;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    public class ViewBase : Page
    {
        private ViewModelBase vmBase;

        public ViewBase() { }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (this.DataContext != null)
            {
                this.vmBase = (ViewModelBase)this.DataContext;
                this.vmBase.AppFrame = this.Frame;
                this.vmBase.NavigateTo(e);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (this.vmBase != null)
            {
                this.vmBase.NavigateFrom(e);
            }
        }
    }
}
