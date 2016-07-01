namespace syncronization
{
    using common.Common;
    using common.Services.Database;
    using common.Tiles;
    using System.Threading.Tasks;
    using Windows.ApplicationModel.Background;

    public sealed class TileSyncronizationTask : IBackgroundTask
    {

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            if (!Utils.UserIsLogged())
            {
                return;
            }

            DbService dbService = new DbService();

            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            await RefreshTile(dbService);

            deferral.Complete();
        }

        private async Task RefreshTile(DbService dbService)
        {
            await TileGenerator.Generate(dbService, false);
        }
    }
}