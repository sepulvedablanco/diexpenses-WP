namespace diexpenses.Common
{
    using common.Common;
    using common.Entities;
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
            settings.Values[Constants.LOGGED_USER_ID] = user.Id;
            settings.Values[Constants.LOGGED_USER_NAME] = user.Name;

            PasswordVault vault = new PasswordVault();
            PasswordCredential credential = new PasswordCredential(Constants.PASSWORD_CREDENTIAL, user.Username, user.AuthToken);
            vault.Add(credential);
        }

        public static void SaveTileIdInMemory(String tileId)
        {
            var settings = ApplicationData.Current.LocalSettings;
            settings.Values[Constants.TILE_ID] = tileId;
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

        public static string GetLoggedUserName()
        {
            var Settings = ApplicationData.Current.LocalSettings;
            object userName = Settings.Values[Constants.LOGGED_USER_NAME];
            return userName == null ? null : userName.ToString();
        }

        public static string GetTileId()
        {
            var settings = ApplicationData.Current.LocalSettings;
            object tileId = settings.Values[Constants.TILE_ID];
            return tileId == null ? null : tileId.ToString();
        }

        public static bool IsValidNumber(string value, int length)
        {
            if(string.IsNullOrEmpty(value))
            {
                return false;
            }
            if(value.Length != length)
            {
                return false;
            }
            try
            {
                Convert.ToInt64(value);
            } catch(Exception)
            {
                return false;
            }
            return true;
        }

        public static bool IsValidBankAccount(string iban, string entity, string office, string controlDigit, string accountNumber)
        {
            if(GetControlDigit("00" + entity + office) + GetControlDigit(accountNumber) == controlDigit)
            {
                return IsIbanChecksumValid(iban + entity + office + controlDigit + accountNumber);
            }
            return false;
        }

        private static string GetControlDigit(string value)
        {
            int[] pesos = { 1, 2, 4, 8, 5, 10, 9, 7, 3, 6 };
            UInt32 suma = 0;
            UInt32 resto;

            for (int i = 0; i < pesos.Length; i++)
            {
                suma += (UInt32)pesos[i] * UInt32.Parse(value.Substring(i, 1));
            }
            resto = 11 - (suma % 11);

            if (resto == 10) resto = 1;
            if (resto == 11) resto = 0;

            return resto.ToString("0");
        }

        private static bool IsIbanChecksumValid(string iban)
        {
            if (iban.Length < 4 || iban[0] == ' ' || iban[1] == ' ' || iban[2] == ' ' || iban[3] == ' ') throw new InvalidOperationException();

            var checksum = 0;
            var ibanLength = iban.Length;
            for (int charIndex = 0; charIndex < ibanLength; charIndex++)
            {
                if (iban[charIndex] == ' ') continue;

                int value;
                var c = iban[(charIndex + 4) % ibanLength];
                if ((c >= '0') && (c <= '9'))
                {
                    value = c - '0';
                }
                else if ((c >= 'A') && (c <= 'Z'))
                {
                    value = c - 'A';
                    checksum = (checksum * 10 + (value / 10 + 1)) % 97;
                    value %= 10;
                }
                else if ((c >= 'a') && (c <= 'z'))
                {
                    value = c - 'a';
                    checksum = (checksum * 10 + (value / 10 + 1)) % 97;
                    value %= 10;
                }
                else throw new InvalidOperationException();

                checksum = (checksum * 10 + value) % 97;
            }
            return checksum == 1;
        }
    }
}
