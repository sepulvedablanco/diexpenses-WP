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
            SendData(dbService);

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

        private void SendData(DbService dbService)
        {
            //IList<Kind> lstKinds = dbService.SelectKinds();

            //IList<Subkind> lstSubinds = dbService.SelectSubkinds();

            //IList<BankAccount> lstBankAccounts = dbService.SelectBankAccounts();
        }
    }
}
