namespace diexpenses.Services.Database
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

    }
}
