using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using LostAndFound_API.Domain.Models.Identity;

namespace LostAndFound_API.Persistence.Repositories
{
    public class ApplicationUserManager : UserManager<User>
    {
        public ApplicationUserManager(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor,
           IPasswordHasher<User> passwordHasher, IEnumerable<IUserValidator<User>> userValidators,
           IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer,
           IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger)
           : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public override async Task<User?> FindByEmailAsync(string email)
        {
            var query = base.Users.AsQueryable();

            var user = await query.Include(ur => ur.UserRoles).ThenInclude(r => r.Role)
                .FirstOrDefaultAsync(x => x.Email == email.Trim());

            return (user != null) ? user : null;
        }

        public async Task<User?> FindByIdAsync(long userId)
        {
            var query = base.Users.AsQueryable();

            var user = await query.Include(ur => ur.UserRoles).ThenInclude(r => r.Role)
                    .FirstOrDefaultAsync(u => u.Id == userId);

            return (user != null) ? user : null;
        }

        public async Task<IEnumerable<User>> ListAsync()
        {
            var users = await base.Users
                .Include(ur => ur.UserRoles)
                .ThenInclude(r => r.Role)
                .ToListAsync();

            return users;
        }
    }
}
