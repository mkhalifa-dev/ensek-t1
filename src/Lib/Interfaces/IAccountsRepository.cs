using Lib.Models;

namespace Lib.Interfaces
{
    public interface IAccountsRepository
    {
        public Task<Account> GetByIdAsync(int id);
    }
}
