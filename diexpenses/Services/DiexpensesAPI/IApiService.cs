namespace diexpenses.Services.DiexpensesAPI
{
    using Entities;
    using System.Threading.Tasks;

    public interface IApiService
    {
        Task<User> Login(string user, string password);
    }
}
