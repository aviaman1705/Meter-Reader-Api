using MeterReaderAPI.Entities;
using Microsoft.AspNetCore.Identity;

namespace MeterReaderAPI.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager; 
        private readonly SignInManager<ApplicationUser> _signInManager;
        public UserRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IdentityResult> Create(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result;
        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            var result = await  _userManager.FindByEmailAsync(email);
            return result;
        }
        public async Task<ApplicationUser> GetUserByName(string username)
        {
            var result = await _userManager.FindByNameAsync(username);
            return result;
        }

        public async Task<SignInResult> Login(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, false, false);
            return result;
        }

        public async Task<IdentityResult> Update(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            return result;
        }
    }
}
