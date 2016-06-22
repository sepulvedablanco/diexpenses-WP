namespace diexpenses.Common
{
    using diexpenses.Entities;
    using System;
    using System.Diagnostics;
    using Windows.Security.Credentials;
    using Windows.Storage;

    public class Utils
    {

        public static void SaveUserDataInMemory(User user)
        {
            var settings = ApplicationData.Current.LocalSettings;
            settings.Values[Constants.IS_LOGGED] = true;
            settings.Values[Constants.LOGGED_USER_NAME] = user.Name;

            PasswordVault vault = new PasswordVault();
            PasswordCredential credential = new PasswordCredential(Constants.PASSWORD_CREDENTIAL, user.Username, user.AuthToken);
            vault.Add(credential);
        }

        public static void DeleteUserDataInMemory()
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

        public static bool UserIsLogged()
        {
            var Settings = ApplicationData.Current.LocalSettings;
            object isLogged = Settings.Values[Constants.IS_LOGGED];
            if (isLogged == null || !Boolean.Parse(isLogged.ToString()))
            {
                return false;
            }
            return Boolean.Parse(isLogged.ToString());
        }

        public static string GetLoggedUserName()
        {
            var Settings = ApplicationData.Current.LocalSettings;
            object userName = Settings.Values[Constants.LOGGED_USER_NAME];
            return userName == null ? null : userName.ToString();
        }
    }
}
