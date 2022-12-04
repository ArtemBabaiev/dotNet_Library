using Identity.Entities;
using Identity.Models;

namespace Identity.Services.Interfaces
{
    public interface IUserService
    {
        Task<string> RegisterAsync(RegisterModel model);

        Task<AuthenticationModel> GetTokenAsync(TokenRequestModel model);

        Task<string> AddRoleAsync(AddRoleModel model);

        Task<AuthenticationModel> RefreshTokenAsync(string token);

        AppUser GetById(string id);

        bool RevokeToken(string token);

        IEnumerable<AppUser> GetAll();
    }
}
