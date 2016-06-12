namespace diexpenses.Services.DiexpensesAPI
{
    using Common;
    using Entities;
    using Newtonsoft.Json;
    using System;
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

            HttpClient client = GetDefaultClient();

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

            HttpClient client = GetDefaultClient();

            string json = JsonConvert.SerializeObject(new NewUser(name, username, password));

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


        private HttpClient GetDefaultClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.AcceptLanguage.Add(new HttpLanguageRangeWithQualityHeaderValue("en"));
            client.DefaultRequestHeaders.UserAgent.Add(new HttpProductInfoHeaderValue("Diexpenses Windows Phone"));
            return client;
        }
    }
}
