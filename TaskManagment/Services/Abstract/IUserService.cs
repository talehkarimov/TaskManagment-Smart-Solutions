using System.Collections.Generic;
using TaskManagment.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagment.ViewModel;

namespace TaskManagment.Services.Abstract
{
    public interface IUserService
    {
        Task<ApplicationUser> GetByUsernameAsync(string username);
        bool UserCanLogin(string username);
        void SetUserLastLoginDate(string userId);
        Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
        Task<bool> AddUserWithRoleAsync(AccountViewModel viewModel, string role);
        bool EmailExists(string email);
        bool UserNameExists(string userName);
        Task<ApplicationUser> GetAuthorizedUserAsync(ClaimsPrincipal user);
        Task<IList<ApplicationUser>> GetUsersByRoleAsync(string role);
        Task<ApplicationUser?> GetById(string userId);
    }
}
