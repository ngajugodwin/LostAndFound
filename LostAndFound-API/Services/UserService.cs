using LostAndFound_API.Domain.Models;
using LostAndFound_API.Domain.Models.Identity;
using LostAndFound_API.Domain.Repositories;
using LostAndFound_API.Domain.Services;
using LostAndFound_API.Domain.Services.Communication;
using LostAndFound_API.Persistence.Repositories;
using LostAndFound_API.Services.Constants;
using Microsoft.AspNetCore.Identity;

namespace LostAndFound_API.Services
{
    public class UserService : IUserService
    {
        private readonly RoleManager<Domain.Models.Identity.Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationUserManager _applicationUserManager;
        private readonly IGenericRepository _genericRepository;

        public UserService(IUnitOfWork unitOfWork, RoleManager<Domain.Models.Identity.Role> roleManager,
                           SignInManager<User> signInManager,
                           ApplicationUserManager applicationUserManager, IGenericRepository genericRepository)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _applicationUserManager = applicationUserManager;
            _genericRepository = genericRepository;
        }

        public async Task<UserResponse> SaveAsync(User user, string password)
        {
            try
            {
                user.UserName = user.FullName.Replace(" ", "").ToLower();
                user.CreatedAt = DateTime.Now;
                user.Email = user.Email.Trim();
                password = password.Trim();
                var result = await _applicationUserManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    var savedUser = await _applicationUserManager.FindByIdAsync(user.Id);

                    if (savedUser != null)
                        await _applicationUserManager.AddToRoleAsync(savedUser, RoleName.USER);

                    await _genericRepository.AddAsync<UserNotificationSetting>(new UserNotificationSetting
                    {
                        Status = false,
                        UserId = user.Id
                    });

                    await _unitOfWork.CompleteAsync();

                    return new UserResponse(user);
                }

                return new UserResponse("Failed to create user", result.Errors);

            }
            catch (Exception ex)
            {
                // Do some logging
                return new UserResponse($"An error occurred while saving the user: {ex.Message}");
            }
        }

        public async Task<UserResponse> GetUserByIdAsync(long id)
        {
            var user = await _applicationUserManager.FindByIdAsync(id);

            if (user == null)
                return new UserResponse("User not found");

            return new UserResponse(user);
        }

        public async Task<IEnumerable<User>> ListAsync()
        {
            return await _applicationUserManager.ListAsync();
        }
    }
}
