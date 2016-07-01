namespace common.Services.DiexpensesAPI
{
    using Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IApiService
    {
        Task<User> Login(string user, string password);

        Task<User> Register(string name, string username, string password);

        Task<IList<Kind>> GetKinds();

        Task<IList<Subkind>> GetSubkinds(int kindId);

        Task<IList<BankAccount>> GetBankAccounts();

        Task<IList<Movement>> GetMovements();

        Task<IList<Movement>> GetMovements(int year, int month);

        /*
        Task SaveKind(string description);
        */
    }
}
