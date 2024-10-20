using MeterReaderAPI.Entities;
using Microsoft.AspNetCore.Identity;

namespace MeterReaderAPI.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager; private readonly SignInManager<IdentityUser> _signInManager;
        public UserRepository(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IdentityResult> Create(IdentityUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result;
        }

        public async Task<IdentityUser> GetUserByEmail(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            return result;
        }
        public async Task<IdentityUser> GetUserByName(string username)
        {
            var result = await _userManager.FindByNameAsync(username);
            return result;
        }

        public async Task<SignInResult> Login(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, false, false);
            return result;
        }
    }
}
