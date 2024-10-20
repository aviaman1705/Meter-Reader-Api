using MeterReaderAPI.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MeterReaderAPI.Services
{
    public interface IUserRepository
    {
        Task<IdentityResult> Create(IdentityUser user,string password);
        Task<Microsoft.AspNetCore.Identity.SignInResult> Login(string email, string password);
        Task<IdentityUser> GetUserByEmail(string email);
        Task<IdentityUser> GetUserByName(string username);
    }
}
