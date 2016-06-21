namespace diexpenses.Services.GpsService
{
    using System;
    using System.Threading.Tasks;
    using Windows.Devices.Geolocation;

    public class GpsService : IGpsService
    {
        public async Task<Geoposition> GetCurrentGeoposition()
        {
            Geoposition currentPosition = null;

            var result = await Geolocator.RequestAccessAsync();

            switch (result)
            {
                case GeolocationAccessStatus.Allowed:
                    var locator = new Geolocator();
                    locator.DesiredAccuracy = PositionAccuracy.High;
                    currentPosition = await locator.GetGeopositionAsync();
                    break;
                case GeolocationAccessStatus.Denied:
                    break;
                case GeolocationAccessStatus.Unspecified:
                    break;
            }

            return currentPosition;
        }
    }
}
