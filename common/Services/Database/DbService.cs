namespace common.Services.Database
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

        /* Syncronization task */

        public Kind SelectKindByApiId(int apiId)
        {
            using (var cnx = new SQLiteConnection(new SQLitePlatformWinRT(), dbPath))
            {
                try
                {
                    IList<Kind> lstKinds = cnx.Table<Kind>()
                        .Where(kind => kind.ApiId == apiId)
                        .ToList();
                    if (lstKinds == null || lstKinds.Count == 0)
                    {
                        return null;
                    } else
                    {
                        return lstKinds[0];
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void CheckAndInsertKinds(IList<Kind> lstKinds)
        {
            if (lstKinds == null || lstKinds.Count == 0)
            {
                return;
            }

            for (int i = 0; i < lstKinds.Count; i++)
            {
                Kind kind = lstKinds[i];
                if(SelectKindByApiId(kind.ApiId.GetValueOrDefault()) == null)
                {
                    Upsert(kind);
                }
            }
        }

        public Subkind SelectSubkindByApiId(int apiId)
        {
            using (var cnx = new SQLiteConnection(new SQLitePlatformWinRT(), dbPath))
            {
                try
                {
                    IList<Subkind> lstSubkinds = cnx.Table<Subkind>()
                        .Where(subkind => subkind.ApiId == apiId)
                        .ToList();
                    if (lstSubkinds == null || lstSubkinds.Count == 0)
                    {
                        return null;
                    }
                    else
                    {
                        return lstSubkinds[0];
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void CheckAndInsertSubkinds(IList<Subkind> lstSubkinds, int kindId)
        {
            if (lstSubkinds == null || lstSubkinds.Count == 0)
            {
                return;
            }

            for (int i = 0; i < lstSubkinds.Count; i++)
            {
                Subkind subkind = lstSubkinds[i];
                subkind.KindId = kindId;
                if (SelectSubkindByApiId(subkind.ApiId.GetValueOrDefault()) == null)
                {
                    Upsert(subkind);
                }
            }
        }

        public BankAccount SelectBankAccountByApiId(int apiId)
        {
            using (var cnx = new SQLiteConnection(new SQLitePlatformWinRT(), dbPath))
            {
                try
                {
                    IList<BankAccount> lstBankAccounts = cnx.Table<BankAccount>()
                        .Where(bankAccount => bankAccount.ApiId == apiId)
                        .ToList();
                    if (lstBankAccounts == null || lstBankAccounts.Count == 0)
                    {
                        return null;
                    }
                    else
                    {
                        return lstBankAccounts[0];
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void CheckAndInsertBankAccounts(IList<BankAccount> lstBankAccounts)
        {
            if (lstBankAccounts == null || lstBankAccounts.Count == 0)
            {
                return;
            }

            for (int i = 0; i < lstBankAccounts.Count; i++)
            {
                BankAccount bankAccount = lstBankAccounts[i];
                if (SelectBankAccountByApiId(bankAccount.ApiId.GetValueOrDefault()) == null)
                {
                    Upsert(bankAccount);
                }
            }
        }

        public bool SelectMovementByApiId(int apiId)
        {
            using (var cnx = new SQLiteConnection(new SQLitePlatformWinRT(), dbPath))
            {
                try
                {
                    return cnx.Table<Movement>()
                        .Where(movement => movement.ApiId == apiId)
                        .Count() > 0;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void CheckAndInsertMovements(IList<Movement> lstMovements)
        {
            if (lstMovements == null || lstMovements.Count == 0)
            {
                return;
            }

            for (int i = 0; i < lstMovements.Count; i++)
            {
                Movement movement = lstMovements[i];
                if (!SelectMovementByApiId(movement.ApiId.GetValueOrDefault()))
                {
                    var kind = SelectKindByApiId(movement.KindId);
                    var subkind = SelectSubkindByApiId(movement.SubkindId);
                    var bankAccount = SelectBankAccountByApiId(movement.BankAccountId);
                    if(kind == null || kind.Id == null ||subkind == null ||subkind.Id == null || bankAccount == null || bankAccount.Id == null)
                    {
                        continue;
                    }
                    movement.KindId = kind.Id.GetValueOrDefault();
                    movement.SubkindId = subkind.Id.GetValueOrDefault();
                    movement.BankAccountId = bankAccount.Id.GetValueOrDefault();
                    Upsert(movement);
                }
            }
        }

        /*
        public T SelectByApiId<T>(T t, int apiId)
        {
            using (var cnx = new SQLiteConnection(new SQLitePlatformWinRT(), dbPath))
            {
                try
                {
                    IList<T> lstObjects = cnx.Table<T>()
                        .Where(o => o.ApiId == apiId)
                        .ToList();
                    if (lstObjects == null ||lstObjects.Count == 0)
                    {
                        return default(T);
                    }
                    else
                    {
                        return lstObjects[0];
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        */

        /*
        public IList<Kind> SelectKindsWithoutApiId()
        {
            using (var cnx = new SQLiteConnection(new SQLitePlatformWinRT(), dbPath))
            {
                try
                {
                    return cnx.Table<Kind>()
                        .Where(kind => kind.ApiId == null)
                        .ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        */

        /*
        public IList<T> SelectWithoutApiId<T>() where T : new()
        {
            using (var cnx = new SQLiteConnection(new SQLitePlatformWinRT(), dbPath))
            {
                try
                {
                    return cnx.Table<T>()
                        .Where(o => o.ApiId == null)
                        .ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        */

        /* End syncronization task */

    }
}
