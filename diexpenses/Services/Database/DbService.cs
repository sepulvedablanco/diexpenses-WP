namespace diexpenses.Services.Database
{
    using System;
    using System.Linq;
    using Entities;
    using SQLite.Net;
    using Windows.Storage;
    using System.IO;
    using SQLite.Net.Platform.WinRT;
    using System.Collections.Generic;

    public class DbService : IDbService
    {
        private static readonly string dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "diexpenses.sqlite");

        public void CreateDb()
        {
            using (var cnx = new SQLiteConnection(new SQLitePlatformWinRT(), dbPath))
            {
                cnx.CreateTable<Kind>();
                cnx.CreateTable<Subkind>();
                cnx.CreateTable<BankAccount>();
                cnx.CreateTable<Movement>();

                cnx.Commit();
            }
        }

        public IList<Kind> SelectKinds()
        {
            using (var cnx = new SQLiteConnection(new SQLitePlatformWinRT(), dbPath))
            {
                try
                {
                    return cnx.Table<Kind>()
                        .OrderBy(kind => kind.Description)
                        .ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public IList<BankAccount> SelectBankAccounts()
        {
            using (var cnx = new SQLiteConnection(new SQLitePlatformWinRT(), dbPath))
            {
                try
                {
                    return cnx.Table<BankAccount>()
                        .OrderBy(bankAccount => bankAccount.Description)
                        .ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void UpsertKind(Kind item)
        {
            using (var cnx = new SQLiteConnection(new SQLitePlatformWinRT(), dbPath))
            {
                if (cnx.Update(item) == 0)
                    cnx.InsertOrReplace(item);

                cnx.Commit();
            }
        }

        public void UpsertBankAccount(BankAccount item)
        {
            using (var cnx = new SQLiteConnection(new SQLitePlatformWinRT(), dbPath))
            {
                if (cnx.Update(item) == 0)
                    cnx.InsertOrReplace(item);

                cnx.Commit();
            }
        }

        public bool DeleteKind(Kind item)
        {
            using (var cnx = new SQLiteConnection(new SQLitePlatformWinRT(), dbPath))
            {
                int rowsAffected = cnx.Delete(item);
                cnx.Commit();
                return rowsAffected == 1;
            }
        }
    }
}
