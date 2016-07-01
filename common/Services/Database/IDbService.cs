namespace common.Services.Database
{
    using Entities;
    using System.Collections.Generic;

    public interface IDbService
    {
        void CreateDb();

        IList<Kind> SelectKinds();

        IList<Subkind> SelectSubkinds(int kindId);

        IList<BankAccount> SelectBankAccounts();

        IList<Movement> SelectMonthlyMovements(int year, int month);

        double SelectTotalAmount();

        double SelectMonthlAmount(bool expense, int year, int month);

        void Upsert<T>(T item);

        bool Delete<T>(T item);

        /* Syncronization task */

        void CheckAndInsertKinds(IList<Kind> lstKinds);

        void CheckAndInsertSubkinds(IList<Subkind> lstSubkinds, int kindId);

        void CheckAndInsertBankAccounts(IList<BankAccount> lstBankAccounts);

        void CheckAndInsertMovements(IList<Movement> lstMovements);
    }
}
