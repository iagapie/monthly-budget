using System.Collections.Generic;
using System.Threading.Tasks;
using IA.Finance.Domain.SeedWork;

namespace IA.Finance.Domain.AggregatesModel.UserAggregate
{
    public interface IUserRepository : IRepository<User>
    {
        Task Add(string userName, string email, string password, string role = "user", string firstName = null,
            string lastName = null);

        Task Update(User user);

        Task<IEnumerable<User>> Find();

        Task<User> FindById(long userId);

        Task<User> FindByIdentityId(string identityId);
        
        Task<User> FindByUserName(string userName);
        
        Task<User> FindByEmail(string email);

        Task<bool> CheckPassword(User user, string password);

        Task<bool> ChangePassword(string identityId, string currentPassword, string newPassword);
    }
}