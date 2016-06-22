﻿namespace diexpenses.Views
{
    using Base;
    using Entities;
    using ViewModels;

    public sealed partial class MovementDetailsPage : MenuBottomViewBase
    {
        private Movement movement;

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
