using LostAndFound_API.Domain.Models.Identity;
using LostAndFound_API.Persistence.Context;
using Microsoft.AspNetCore.Identity;

namespace LostAndFound_API.Persistence.Seeders
{
    public class Seed
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ApplicationDbContext _contex;

        public Seed(UserManager<User> userManager, RoleManager<Role> roleManager, ApplicationDbContext contex)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _contex = contex;
        }

        public void SeedUsers()
        {

            if (!_userManager.Users.Any())
            {
                var roles = new List<Role>
                {
                    new Role{Name = "User"},
                    new Role{Name = "Admin"},
                };

                foreach (var role in roles)
                {
                    _roleManager.CreateAsync(role).Wait();
                }

                var superAdmin = new User
                {
                    FullName = "Admin",
                    UserName = "admin",
                    PhoneNumber = "12345",
                    Email = "admin@example.com",
                };

                IdentityResult result = _userManager.CreateAsync(superAdmin, "Pa$$w0rd").Result;

                if (result.Succeeded)
                {
                    var admin = _userManager.FindByEmailAsync("admin@example.com").Result;

                    if (admin != null)
                        _userManager.AddToRoleAsync(admin, "Admin").Wait();
                }
            }


        }
    }
}
