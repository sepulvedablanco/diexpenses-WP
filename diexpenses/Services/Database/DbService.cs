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

        public IList<Subkind> SelectSubkinds(int kindId)
        {
            using (var cnx = new SQLiteConnection(new SQLitePlatformWinRT(), dbPath))
            {
                try
                {
                    return cnx.Table<Subkind>()
                        .Where(subkind => subkind.KindId == kindId)
                        .OrderBy(subkind => subkind.Description)
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

        public IList<Movement> SelectMonthlyMovements(int year, int month)
        {
            using (var cnx = new SQLiteConnection(new SQLitePlatformWinRT(), dbPath))
            {
                try
                {
                    var movements = cnx.Table<Movement>()
                                .Where(m => m.Year == year && m.Month == month)
                                .OrderByDescending(movement => movement.TransactionDate)
                                .ToList();

                    List<BankAccount> bankAccounts = cnx.Table<BankAccount>().ToList();
                    movements = movements.Join(bankAccounts, movement => movement.BankAccountId,
                                       bankAccount => bankAccount.Id,
                                       (movement, bankAccount) => { movement.BankAccount = bankAccount; return movement; }).ToList();

                    List<Kind> kinds = cnx.Table<Kind>().ToList();
                    movements = movements.Join(kinds, movement => movement.KindId,
                                       kind => kind.Id,
                                       (movement, kind) => { movement.Kind = kind; return movement; }).ToList();

                    List<Subkind> subkinds = cnx.Table<Subkind>().ToList();
                    movements = movements.Join(subkinds, movement => movement.SubkindId,
                                       subkind => subkind.Id,
                                       (movement, subkind) => { movement.Subkind = subkind; return movement; }).ToList();

                    return movements;

                    /* Seguir intentando hacer el join y si funciona poner columnas de año y mes en el modelo.
                    return cnx.Table<Movement>()
                        .Join<Movement, Kind, Movement, Movement>(cnx.Table<Movement>().ToList(), 
                        movement => movement, 
                        kind => kind.Id, 
                        (movement, kind) => )
                        .Where(m => m.TransactionDate.Year == year && m.TransactionDate.Month == month)
                        .OrderBy(movement => movement.TransactionDate)
                        .ToList();
                     */


                    // ex --> {"Member access failed to compile expression"}
                    /*
                    return cnx.Table<Movement>()
                                .Where(m => m.TransactionDate.Year == year && m.TransactionDate.Month == month)
                                .OrderBy(movement => movement.TransactionDate)
                                .ToList();
                    */

                    // ex --> {"Joins are not supported."} --> .Month is not supported by your LINQ provider
                    /*
                                        var items = from movement in cnx.Table<Movement>()
                                                    join bankAccount in cnx.Table<BankAccount>() on movement.BankAccountId equals bankAccount.Id
                                                    join kind in cnx.Table<Kind>() on movement.KindId equals kind.Id
                                                    join subkind in cnx.Table<Subkind>() on movement.SubkindId equals subkind.Id
                                                    join subkind2 in cnx.Table<Subkind>() on kind.Id equals subkind2.KindId
                                                    let convertedDate = (DateTime)movement.TransactionDate
                                                    where convertedDate.Year == year && convertedDate.Month == month
                                                    select movement;
                                        return items.ToList();
                    */
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public double SelectTotalAmount()
        {
            using (var cnx = new SQLiteConnection(new SQLitePlatformWinRT(), dbPath))
            {
                try
                {
                    return (from bankAccount in cnx.Table<BankAccount>()
                            select bankAccount.Balance).Sum();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public double SelectMonthlAmount(bool expense, int year, int month)
        {
            using (var cnx = new SQLiteConnection(new SQLitePlatformWinRT(), dbPath))
            {
                try
                {
                    return (from movement in cnx.Table<Movement>()
                                where movement.Expense == expense && movement.Year == year && movement.Month == month
                                select movement.Amount).Sum();                 
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void Upsert<T>(T item)
        {
            using (var cnx = new SQLiteConnection(new SQLitePlatformWinRT(), dbPath))
            {
                if (cnx.Update(item) == 0)
                    cnx.InsertOrReplace(item);

                cnx.Commit();
            }
        }

        public bool Delete<T>(T o)
        {
            using (var cnx = new SQLiteConnection(new SQLitePlatformWinRT(), dbPath))
            {
                int rowsAffected = cnx.Delete(o);
                cnx.Commit();
                return rowsAffected == 1;
            }
        }
    }
}
