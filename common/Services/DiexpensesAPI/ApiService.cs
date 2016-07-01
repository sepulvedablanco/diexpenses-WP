namespace common.Services.DiexpensesAPI
{
    using common.Common;
    using common.Entities;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Windows.Storage.Streams;
    using Windows.Web.Http;
    using Windows.Web.Http.Headers;

    public class ApiService : IApiService
    {
        private static readonly string endpoint = "https://diexpenses-herokuapp-com-u8gcrab3473z.runscope.net";

        public async Task<User> Login(string user, string password)
        {
            Uri loginURI = new Uri(endpoint + "/user/login");

            HttpClient client = GetDefaultClient(null);

            string json = JsonConvert.SerializeObject(new Credential(user, password));

            HttpStringContent stringContent = new HttpStringContent(json, UnicodeEncoding.Utf8, Constants.JSON_HEADER);
            HttpResponseMessage response = await client.PostAsync(loginURI, stringContent);

            Debug.WriteLine("Login response = " + response.StatusCode);

            if (response.StatusCode != Windows.Web.Http.HttpStatusCode.Ok || !response.IsSuccessStatusCode)
            {
                return null;
            }

            string content = await response.Content.ReadAsStringAsync();
            User userLogged = JsonConvert.DeserializeObject<User>(content);
            Debug.WriteLine("User logged = " + userLogged.ToString());
            return userLogged;
        }

        public async Task<User> Register(string name, string username, string password)
        {
            Uri registerURI = new Uri(endpoint + "/user");

            HttpClient client = GetDefaultClient(null);

            string json = JsonConvert.SerializeObject(new Entities.NewUser(name, username, password));

            HttpStringContent stringContent = new HttpStringContent(json, UnicodeEncoding.Utf8, Constants.JSON_HEADER);
            HttpResponseMessage response = await client.PostAsync(registerURI, stringContent);

            Debug.WriteLine("Register response = " + response.StatusCode);

            if (response.StatusCode != Windows.Web.Http.HttpStatusCode.Created || !response.IsSuccessStatusCode)
            {
                return null;
            }

            string content = await response.Content.ReadAsStringAsync();
            GenericResponse registerReponse = JsonConvert.DeserializeObject<GenericResponse>(content);
            if(registerReponse == null || registerReponse.Code != 3)
            {
                return null;
            }
            Debug.WriteLine("Register response = " + registerReponse.ToString());
            return await Login(username, password);
        }

        public async Task<IList<Kind>> GetKinds()
        {
            Uri kindsURI = new Uri(endpoint + "/user/" + Utils.GetLoggedUserId() + "/financialMovementTypes");

            HttpClient client = GetDefaultClient(Utils.GetLoggedUserToken());

            HttpResponseMessage response = await client.GetAsync(kindsURI);

            Debug.WriteLine("Kinds response = " + response.StatusCode);

            if (response.StatusCode != Windows.Web.Http.HttpStatusCode.Ok || !response.IsSuccessStatusCode)
            {
                return null;
            }

            string content = await response.Content.ReadAsStringAsync();
            IList<Kind> lstKinds = JsonConvert.DeserializeObject<IList<Kind>>(content);
            Debug.WriteLine("Kinds retrieved = " + lstKinds.Count);
            return Modify(lstKinds);
        }

        private IList<Kind> Modify(IList<Kind> lstKinds)
        {
            if (lstKinds == null || lstKinds.Count == 0)
            {
                return null;
            }
            IList<Kind> lstKindsResult = new List<Kind>();
            for(int i = 0; i < lstKinds.Count; i++)
            {
                Kind kind = lstKinds[i];
                kind.ApiId = kind.Id;
                kind.Id = null;
                lstKindsResult.Add(kind);
            }
            return lstKindsResult;
        }

        public async Task<IList<Subkind>> GetSubkinds(int apiKindId)
        {
            Uri subkindsURI = new Uri(endpoint + "/user/financialMovementType/" + apiKindId + "/financialMovementSubtypes");

            HttpClient client = GetDefaultClient(Utils.GetLoggedUserToken());

            HttpResponseMessage response = await client.GetAsync(subkindsURI);

            Debug.WriteLine("Subkinds response = " + response.StatusCode);

            if (response.StatusCode != Windows.Web.Http.HttpStatusCode.Ok || !response.IsSuccessStatusCode)
            {
                return null;
            }

            string content = await response.Content.ReadAsStringAsync();
            IList<Subkind> lstSubkinds = JsonConvert.DeserializeObject<IList<Subkind>>(content);
            Debug.WriteLine("Subkinds retrieved = " + lstSubkinds.Count);
            return Modify(lstSubkinds);
        }

        private IList<Subkind> Modify(IList<Subkind> lstSubkinds)
        {
            if (lstSubkinds == null || lstSubkinds.Count == 0)
            {
                return null;
            }
            IList<Subkind> lstSubkindsResult = new List<Subkind>();
            for (int i = 0; i < lstSubkinds.Count; i++)
            {
                Subkind subkind = lstSubkinds[i];
                subkind.ApiId = subkind.Id;
                subkind.Id = null;
                lstSubkindsResult.Add(subkind);
            }
            return lstSubkindsResult;
        }

        public async Task<IList<BankAccount>> GetBankAccounts()
        {
            Uri bankAccountsURI = new Uri(endpoint + "/user/" + Utils.GetLoggedUserId() + "/bankAccounts");

            HttpClient client = GetDefaultClient(Utils.GetLoggedUserToken());

            HttpResponseMessage response = await client.GetAsync(bankAccountsURI);

            Debug.WriteLine("Bank Accounts response = " + response.StatusCode);

            if (response.StatusCode != Windows.Web.Http.HttpStatusCode.Ok || !response.IsSuccessStatusCode)
            {
                return null;
            }

            string content = await response.Content.ReadAsStringAsync();
            IList<BankAccount> lstBankAccounts = JsonConvert.DeserializeObject<IList<BankAccount>>(content);
            Debug.WriteLine("Bank accounts retrieved = " + lstBankAccounts.Count);
            return Modify(lstBankAccounts);
        }

        private IList<BankAccount> Modify(IList<BankAccount> lstBankAccounts)
        {
            if (lstBankAccounts == null || lstBankAccounts.Count == 0)
            {
                return null;
            }
            IList<BankAccount> lstBankAccountsResult = new List<BankAccount>();
            for (int i = 0; i < lstBankAccounts.Count; i++)
            {
                BankAccount bankAccount = lstBankAccounts[i];
                bankAccount.ApiId = bankAccount.Id;
                bankAccount.Id = null;
                lstBankAccountsResult.Add(bankAccount);
            }
            return lstBankAccountsResult;
        }

        public async Task<IList<Movement>> GetMovements()
        {
            return await SearchMovements(null, null);
        }

        public async Task<IList<Movement>> GetMovements(int year, int month)
        {
            return await SearchMovements(year, month);
        }

        private async Task<IList<Movement>> SearchMovements(int? year, int? month)
        {
            var queryString = "";
            if(year != null)
            {
                queryString += "?y=" + year;
            }
            if (month != null)
            {
                if (!queryString.StartsWith("?"))
                {
                    queryString = "?";
                }
                queryString += "m=" + month;
            }
            Uri movementsURI = new Uri(endpoint + "/user/" + Utils.GetLoggedUserId() + "/financialMovements" + queryString);

            HttpClient client = GetDefaultClient(Utils.GetLoggedUserToken());

            HttpResponseMessage response = await client.GetAsync(movementsURI);

            Debug.WriteLine("Movements response = " + response.StatusCode);

            if (response.StatusCode != Windows.Web.Http.HttpStatusCode.Ok || !response.IsSuccessStatusCode)
            {
                return null;
            }

            string content = await response.Content.ReadAsStringAsync();
            var format = "dd/MM/yyyy HH:mm:ss";
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
            MovementsPage movementPage = JsonConvert.DeserializeObject<MovementsPage>(content, dateTimeConverter);
            IList<Movement> lstMovements = movementPage.Movements;
            Debug.WriteLine("Movements retrieved = " + lstMovements.Count + " - " + movementPage.TotalMovements);
            return Modify(lstMovements);
        }

        private IList<Movement> Modify(IList<Movement> lstMovements)
        {
            if (lstMovements == null || lstMovements.Count == 0)
            {
                return null;
            }
            IList<Movement> lstMovementsResult = new List<Movement>();
            for (int i = 0; i < lstMovements.Count; i++)
            {
                Movement movement = lstMovements[i];
                movement.ApiId = movement.Id;
                movement.Id = null;
                lstMovementsResult.Add(movement);
            }
            return lstMovementsResult;
        }

        private HttpClient GetDefaultClient(String token)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.AcceptLanguage.Add(new HttpLanguageRangeWithQualityHeaderValue("en"));
            client.DefaultRequestHeaders.UserAgent.Add(new HttpProductInfoHeaderValue("Diexpenses Windows Phone"));
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Add("X-Auth-Token", token);
            }
            return client;
        }
    }
}
