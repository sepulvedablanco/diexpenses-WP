namespace diexpenses.Services.GpsService
{
    using System.Threading.Tasks;
    using Windows.Devices.Geolocation;

    public interface IGpsService
    {
        Task<Geoposition> GetCurrentGeoposition();
    }
}
