namespace common.Services.NetworkService
{
    using System;

    public interface INetworkService
    {
        event EventHandler<EventArgs> NetworkAvailabilityChanged;

        bool IsNetworkAvailable { get; }
    }
}
