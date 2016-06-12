namespace diexpenses.Common
{
    using diexpenses.Entities;
    using Windows.Security.Credentials;
    using Windows.Storage;

    class Utils
    {

        public static void SaveDataInMemory(User user)
        {
            var settings = ApplicationData.Current.LocalSettings;
            settings.Values[Constants.IS_LOGGED] = true;

            PasswordVault vault = new PasswordVault();
            PasswordCredential credential = new PasswordCredential(Constants.PASSWORD_CREDENTIAL, user.Username, user.AuthToken);
            vault.Add(credential);
        }
    }
}
