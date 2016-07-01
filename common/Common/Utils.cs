namespace common.Common
{
    using System;
    using Windows.ApplicationModel.Background;
    using Windows.Security.Credentials;
    using Windows.Storage;

    public class Utils
    {
        public static bool UserIsLogged()
        {
            return GetBoolSetting(Constants.IS_LOGGED, false);
        }

        public static bool IsFirstDataSyncronization()
        {
            return GetBoolSetting(Constants.IS_FIRST_DATA_SYNCRONIZATION, true);
        }

        public static bool GetBoolSetting(string name, bool defaultValue)
        {
            var Settings = ApplicationData.Current.LocalSettings;
            object value = Settings.Values[name];
            if (value == null || !Boolean.Parse(value.ToString()))
            {
                return defaultValue;
            }
            return Boolean.Parse(value.ToString());
        }

        public static string GetLoggedUserId()
        {
            var Settings = ApplicationData.Current.LocalSettings;
            object userId = Settings.Values[Constants.LOGGED_USER_ID];
            return userId == null ? null : userId.ToString();
        }

        public static string GetLoggedUserToken()
        {
            PasswordVault vault = new PasswordVault();
            var lstPasswordCredential = vault.FindAllByResource(Constants.PASSWORD_CREDENTIAL);
            if (lstPasswordCredential == null || lstPasswordCredential.Count == 0)
            {
                return null;
            }
            var passwordCredential = lstPasswordCredential[0];
            passwordCredential.RetrievePassword();
            return passwordCredential.Password;
        }

        public static void RegisterTaskIfNeeded(string taskName, string entryPoint)
        {
            if (!IsBackGroundTaskRegistered(taskName))
            {
                RegisterBackgroundTask(taskName, entryPoint);
            }
        }

        private static bool IsBackGroundTaskRegistered(string taskName)
        {
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == taskName)
                {
                    return true;
                }
            }
            return false;
        }

        private static void RegisterBackgroundTask(string taskName, string entryPoint)
        {
            var builder = new BackgroundTaskBuilder();
            builder.Name = taskName;
            builder.TaskEntryPoint = entryPoint;
            builder.SetTrigger(new TimeTrigger(30, false));
            BackgroundTaskRegistration task = builder.Register();
        }
    }
}
