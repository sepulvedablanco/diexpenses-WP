namespace diexpenses.Services.Database
{
    using Entities;
    using System.Collections.Generic;

    public interface IDbService
    {
        void CreateDb();

        IList<Kind> SelectKinds();

        IList<BankAccount> SelectBankAccounts();

        void UpsertKind(Kind item);

        void UpsertBankAccount(BankAccount item);

        bool DeleteKind(Kind item);
    }
}
