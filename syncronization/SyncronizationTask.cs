namespace syncronization
{
    using common.Common;
    using common.Entities;
    using common.Services.Database;
    using common.Services.DiexpensesAPI;
    using common.Services.NetworkService;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Windows.ApplicationModel.Background;

    public sealed class SyncronizationTask : IBackgroundTask
    {

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            NetworkService networkService = new NetworkService();

            if (!Utils.UserIsLogged() || !networkService.IsNetworkAvailable)
            {
                return;
            }

            ApiService apiService = new ApiService();
            DbService dbService = new DbService();

            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            await RetrieveData(apiService, dbService);
            //await SendData(dbService, apiService);

            deferral.Complete();
        }

        private async Task RetrieveData(ApiService apiService, DbService dbService)
        {
            IList<Kind> lstKinds = await apiService.GetKinds();
            dbService.CheckAndInsertKinds(lstKinds);

            if (lstKinds != null)
            {
                for (int i = 0; i < lstKinds.Count; i++)
                {
                    IList<Subkind> lstSubkinds = await apiService.GetSubkinds(lstKinds[i].ApiId.GetValueOrDefault());
                    dbService.CheckAndInsertSubkinds(lstSubkinds, lstKinds[i].Id.GetValueOrDefault());
                }
            }

            IList<BankAccount> lstBankAccounts = await apiService.GetBankAccounts();
            dbService.CheckAndInsertBankAccounts(lstBankAccounts);

            IList<Movement> lstMovements;
            if (Utils.IsFirstDataSyncronization())
            {
                lstMovements = await apiService.GetMovements();
            }
            else
            {
                var today = DateTime.Today;
                lstMovements = await apiService.GetMovements(today.Year, today.Month);
            }
            dbService.CheckAndInsertMovements(lstMovements);
        }

        // app to server syncrhonization is not allowed because when you insert an object in the api, this does not return the object id, so, then you can update de apiId in the app.
        // in a near future the api will be modify to return the inserted object id.
        /* 
        private async Task SendData(DbService dbService, ApiService apiService)
        {
            IList<Kind> lstKinds = dbService.SelectKindsWithoutApiId();
            if(lstKinds != null && lstKinds.Count > 0)
            {
                for (int i = 0; i < lstKinds.Count; i++)
                {
                    await apiService.SaveKind(lstKinds[i].Description);
                }
            }
        
        }
        */
    }
}
