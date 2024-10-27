using MeterReaderAPI.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MeterReaderAPI.Services
{
    public interface IUserRepository
    {
        Task<IdentityResult> Create(ApplicationUser user,string password);
        Task<Microsoft.AspNetCore.Identity.SignInResult> Login(string email, string password);
        Task<ApplicationUser> GetUserByEmail(string email);
        Task<ApplicationUser> GetUserByName(string username);
        Task<IdentityResult>Update(ApplicationUser user);
    }
}
