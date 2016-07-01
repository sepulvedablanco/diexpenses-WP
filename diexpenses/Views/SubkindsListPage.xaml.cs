namespace diexpenses.Views
{
    using Base;
    using common.Entities;
    using ViewModels;

    public sealed partial class SubkindsListPage : MenuBottomViewBase
    {
        public SubkindsListPage()
        {
            this.InitializeComponent();
        }

        public SubkindsListPage(Kind kind)
        {
            this.InitializeComponent();

            SubkindsListPageViewModel.Kind = kind;
        }

    }
}
