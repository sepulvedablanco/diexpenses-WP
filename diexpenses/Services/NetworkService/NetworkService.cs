namespace diexpenses.Services.NetworkService
{
    using System;
    using Windows.Networking.Connectivity;

    public class NetworkService : INetworkService
    {
        public NetworkService()
        {
            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
        }

        public bool IsNetworkAvailable
        {
            get
            {
                var profile = NetworkInformation.GetInternetConnectionProfile();

                if (profile != null)
                    if (profile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess)
                        return true;

                return false;
            }
        }

        public event EventHandler<EventArgs> NetworkAvailabilityChanged;

        private void NetworkInformation_NetworkStatusChanged(object sender)
        {
            NetworkAvailabilityChanged?.Invoke(this, new EventArgs());
        }
    }
}
