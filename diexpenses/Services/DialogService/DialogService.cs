namespace diexpenses.Services
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Windows.UI.Popups;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Data;

    public class DialogService : IDialogService
    {
        public async void ShowAlert(string message)
        {
            MessageDialog dialog = new MessageDialog(message);
            await dialog.ShowAsync();
        }

        public async Task<string> ShowMessage(string title, string message, string tbText, string primaryButtonText, string secondaryButtonText)
        {
            var result = await GetDialog(title, message, true, tbText, false, null, primaryButtonText, secondaryButtonText);
            var strResult = result as string;
            Debug.WriteLine(strResult);
            return strResult;
        }

        public async Task<bool> ShowConfirmMessage(string title, string message, string cbText, string primaryButtonText, string secondaryButtonText)
        {
            var result = await GetDialog(title, message, false, null, true, cbText, primaryButtonText, secondaryButtonText);
            var bResult = result != null ? (bool)result : false;
            Debug.WriteLine(bResult);
            return bResult;
        }

        private async Task<object> GetDialog(string title, string message, bool showTb, string tbText, bool showCb, string cbText, string primaryButtonText, string secondaryButtonText)
        {
            var dialog = new ContentDialog()
            {
                Title = title,
                RequestedTheme = ElementTheme.Dark 
            };

            var panel = new StackPanel();

            panel.Children.Add(new TextBlock
            {
                Text = message,
                TextWrapping = TextWrapping.Wrap,
            });

            TextBox tb = null;

            if (showTb)
            {
                if (tbText == null)
                {
                    tb = new TextBox();
                }
                else
                {
                    tb = new TextBox
                    {
                        Text = tbText
                    };
                }

                Thickness margin = tb.Margin;
                margin.Top = 10;
                tb.Margin = margin;

                tb.TextChanged += delegate
                {
                    dialog.IsPrimaryButtonEnabled = tb.Text.Length > 0;
                };
                panel.Children.Add(tb);
            }

            CheckBox cb = null;

            if (showCb)
            {
                if (cbText == null)
                {
                    cb = new CheckBox();
                }
                else
                {
                    cb = new CheckBox
                    {
                        Content = cbText
                    };
                }

                cb.SetBinding(CheckBox.IsCheckedProperty, new Binding
                {
                    Source = dialog,
                    Path = new PropertyPath("IsPrimaryButtonEnabled"),
                    Mode = BindingMode.TwoWay,
                });

                panel.Children.Add(cb);
            }

            dialog.Content = panel;
            dialog.PrimaryButtonText = primaryButtonText;
            dialog.IsPrimaryButtonEnabled = false;
            dialog.SecondaryButtonText = secondaryButtonText;

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                if (tb != null && !string.IsNullOrEmpty(tb.Text))
                {
                    return tb.Text;
                }
                else if (cb != null)
                {
                    return cb.IsChecked;
                }
            }
            return null;
        }
    }
}
