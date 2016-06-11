namespace diexpenses.Services.Database
{
    using Entities;
    using System.Collections.Generic;

    public interface IDbService
    {
        void CreateDb();

        void UpsertKind(Kind item);

        IList<Kind> SelectKinds();
    }
}
