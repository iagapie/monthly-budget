using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IA.Finance.Domain.AggregatesModel.UserAggregate;
using IA.Finance.Domain.Exceptions;
using IA.Finance.Domain.SeedWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IA.Finance.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly FinanceContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public UserRepository(UserManager<IdentityUser> userManager, FinanceContext context)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task Add(string userName, string email, string password, string role = "user", string firstName = null,
            string lastName = null)
        {
            var identityUser = new IdentityUser {Email = email, UserName = userName};

            var identityResult = await _userManager.CreateAsync(identityUser, password).ConfigureAwait(false);

            if (identityResult.Succeeded)
            {
                var user = new User(identityUser.UserName, identityUser.Email, identityUser.Id, role, firstName, lastName);
                _context.Users.Add(user);
            }
            
            await UnitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Update(User user)
        {
            var identityUser = await _userManager.FindByIdAsync(user.IdentityId);

            if (identityUser.Email != user.Email || identityUser.UserName != user.UserName)
            {
                identityUser.Email = user.Email;
                identityUser.UserName = user.UserName;

                await _userManager.UpdateAsync(identityUser).ConfigureAwait(false);
            }
            
            _context.Entry(user).State = EntityState.Modified;

            await UnitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<User>> Find() =>
            await _context.Users
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync()
                .ConfigureAwait(false);

        public async Task<User> FindById(long userId) => await _context.Users.FindAsync(userId).ConfigureAwait(false);

        public async Task<User> FindByIdentityId(string identityId)
        {
            var identity = await _userManager.FindByIdAsync(identityId).ConfigureAwait(false);
            return identity == null ? null : await FindInternalById(identity.Id).ConfigureAwait(false);
        }

        public async Task<User> FindByUserName(string userName)
        {
            var identity = await _userManager.FindByNameAsync(userName).ConfigureAwait(false);
            return identity == null ? null : await FindInternalById(identity.Id).ConfigureAwait(false);
        }

        public async Task<User> FindByEmail(string email)
        {
            var identity = await _userManager.FindByEmailAsync(email).ConfigureAwait(false);
            return identity == null ? null : await FindInternalById(identity.Id).ConfigureAwait(false);
        }

        public async Task<bool> CheckPassword(User user, string password)
        {
            //var identity = new IdentityUser {Id = user.IdentityId, Email = user.Email, UserName = user.UserName};
            var identity = await _userManager.FindByIdAsync(user.IdentityId).ConfigureAwait(false);
            return await _userManager.CheckPasswordAsync(identity, password).ConfigureAwait(false);
        }

        public async Task<bool> ChangePassword(string identityId, string currentPassword, string newPassword)
        {
            var identity = await _userManager.FindByIdAsync(identityId).ConfigureAwait(false);

            if (identity == null)
            {
                throw new FinanceDomainException(_userManager.ErrorDescriber.DefaultError().Description);
            }

            var result = await _userManager.ChangePasswordAsync(identity, currentPassword, newPassword)
                .ConfigureAwait(false);

            if (!result.Succeeded)
            {
                throw new FinanceDomainException(result.Errors.First().Description);
            }

            return true;
        }

        private async Task<User> FindInternalById(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.IdentityId == id).ConfigureAwait(false);

            if (user != null)
            {
                await _context.Entry(user).Collection(e => e.RefreshTokens).LoadAsync().ConfigureAwait(false);
            }

            return user;
        }
    }
}