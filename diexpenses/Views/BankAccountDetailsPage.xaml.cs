namespace diexpenses.Views
{
    using Base;
    using common.Entities;
    using ViewModels;

    public sealed partial class BankAccountDetailsPage : MenuBottomViewBase
    {
        public BankAccountDetailsPage()
        {
            this.InitializeComponent();
        }

        public BankAccountDetailsPage(BankAccount bankAccount)
        {
            this.InitializeComponent();

            BankAccountDetailsPageViewModel.BankAccount = bankAccount;
        }
    }
}
