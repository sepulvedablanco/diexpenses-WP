namespace diexpenses.Common
{
    using Microsoft.Xaml.Interactivity;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls.Primitives;
    using Windows.UI.Xaml.Input;

    public class OpenMenuFlyoutAction : DependencyObject, IAction
    {
        private static object holdedObject;

        public object Execute(object sender, object parameter)
        {
            var eventArgs = parameter as HoldingRoutedEventArgs;
            if (eventArgs == null)
            {
                return null;
            }

            var element = eventArgs.OriginalSource as FrameworkElement;
            if (element == null)
            {
                return null;
            }

            if (!element.BaseUri.LocalPath.Contains("MovementDetailsPage.xaml")) // Don't display de flyout if holding comes from movement details page
            {
                FrameworkElement senderElement = sender as FrameworkElement;
                FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
                flyoutBase.ShowAt(senderElement);

                HoldedObject = element.DataContext;
            }

            return null;
        }

        public static object HoldedObject
        {
            get { return holdedObject; }
            set
            {
                holdedObject = value;
            }
        }

    }

}

