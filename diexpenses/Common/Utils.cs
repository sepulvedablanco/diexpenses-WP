namespace diexpenses.Common
{
    using diexpenses.Entities;
    using System.Diagnostics;
    using Windows.Security.Credentials;
    using Windows.Storage;

    public class Utils
    {

        public static void SaveDataInMemory(User user)
        {
            var settings = ApplicationData.Current.LocalSettings;
            settings.Values[Constants.IS_LOGGED] = true;

            PasswordVault vault = new PasswordVault();
            PasswordCredential credential = new PasswordCredential(Constants.PASSWORD_CREDENTIAL, user.Username, user.AuthToken);
            vault.Add(credential);
        }

        public static void DeleteDataInMemory()
        {
            var settings = ApplicationData.Current.LocalSettings;
            settings.Values[Constants.IS_LOGGED] = false;

            PasswordVault vault = new PasswordVault();
            var lstPasswordCredential = vault.FindAllByResource(Constants.PASSWORD_CREDENTIAL);
            if(lstPasswordCredential == null || lstPasswordCredential.Count == 0)
            {
                // There should be one credential.
                return;
            }

            Debug.WriteLine("Number of credentials = " + lstPasswordCredential.Count);
            var credential = lstPasswordCredential[0];
            vault.Remove(credential);
        }
    }
}
