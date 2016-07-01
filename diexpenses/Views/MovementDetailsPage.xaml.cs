namespace diexpenses.Views
{
    using Base;
    using common.Entities;
    using ViewModels;

    public sealed partial class MovementDetailsPage : MenuBottomViewBase
    {
        public MovementDetailsPage()
        {
            this.InitializeComponent();
        }

        public MovementDetailsPage(Movement movement)
        {
            this.InitializeComponent();

            MovementDetailsPageViewModel.Movement = movement;
        }
    }
}
