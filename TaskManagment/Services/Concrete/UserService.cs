using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using TaskManagment.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagment.Data;
using TaskManagment.Services.Abstract;
using TaskManagment.ViewModel;

namespace TaskManagment.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _dbContext;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public UserService(UserManager<ApplicationUser> userManager,
       RoleManager<ApplicationRole> roleManager,
       AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async Task<ApplicationUser> GetByUsernameAsync(string username)
        {
            if (string.IsNullOrEmpty(username)) return null;
            ApplicationUser user = await _userManager.FindByNameAsync(username);
            return user == null || user.IsDeleted ? null : user;
        }

        public bool UserCanLogin(string username)
        {
            ApplicationUser appUser = _dbContext.Users.FirstOrDefault(user => user.UserName.Equals(username));
            if (appUser == null) return false;
            return !appUser.IsDeleted && appUser.HasAccessToLogin;
        }

        public void SetUserLastLoginDate(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return;
            ApplicationUser appUser = _dbContext.Users.FirstOrDefault(user => user.Id.Equals(userId));
            if (appUser == null) return;

            appUser.LastLoginDate = DateTime.Now;
            _dbContext.Users.Update(appUser);
            _dbContext.SaveChanges();
        }
        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            if (user == null) return null;
            return await _userManager.GetRolesAsync(user);
        }

        public bool EmailExists(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;
            return _dbContext.Users.SingleOrDefault(u => u.Email.Equals(email)) != null;
        }

        public bool UserNameExists(string login)
        {
            if (string.IsNullOrEmpty(login)) return false;
            return _dbContext.Users.SingleOrDefault(u => u.UserName.Equals(login)) != null;
        }

        public Task<ApplicationUser> GetAuthorizedUserAsync(ClaimsPrincipal user)
        {
            return _userManager.GetUserAsync(user);
        }
        public async Task<IList<ApplicationUser>> GetUsersByRoleAsync(string role)
        {
            IList<ApplicationUser> usersByRole = await _userManager.GetUsersInRoleAsync(role);
            return usersByRole.Where(u => !u.IsDeleted).ToList();
        }
        public async Task<ApplicationUser?> GetById(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return null;
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            return user == null || user.IsDeleted ? null : user;
        }

        public async Task<bool> AddUserWithRoleAsync(AccountViewModel viewModel, string role)
        {
            if (viewModel == null || EmailExists(viewModel.Email) || UserNameExists(viewModel.Username) ||
                string.IsNullOrEmpty(role)) return false;
            bool roleExists = await _roleManager.RoleExistsAsync(role);
            if (!roleExists) return false;


            var user = new ApplicationUser
            {
                Name = (role == SystemRoles.EmployeeRole ? viewModel.Name : viewModel.OrganizationName),
                Surname = (role == SystemRoles.EmployeeRole ? viewModel.Surname : null),
                UserName = viewModel.Username,
                Email = viewModel.Email,
                IsDeleted = false,
                HasAccessToLogin = true,
                Organization = (role == SystemRoles.EmployeeRole ? null : new Organization()),
                OrganizationId = (role == SystemRoles.EmployeeRole ? viewModel.OrganizationId : null),
            };

            if (role == SystemRoles.OrganizationRole)
            {
                Organization organization = new Organization()
                {
                    Name = viewModel.OrganizationName,
                    Address = viewModel.Address,
                    PhoneNumber = viewModel.PhoneNumber
                };

                user.Organization = organization;
            }

            //Employees must be default password 
            string password = (role == SystemRoles.EmployeeRole ? "emp123" : viewModel.Password);
            IdentityResult result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded) return false;
            ApplicationUser createdUser = await _userManager.FindByNameAsync(user.UserName);
            IdentityResult roleAddingResult = await _userManager.AddToRoleAsync(createdUser, role);
            return roleAddingResult.Succeeded;
        }
    }
}
