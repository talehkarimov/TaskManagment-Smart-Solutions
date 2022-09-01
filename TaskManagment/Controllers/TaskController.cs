using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagment.Models;
using TaskManagment.Services.Abstract;
using TaskManagment.ViewModel;

namespace TaskManagment.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;

        public TaskController(ITaskService taskService, IUserService userService, IOrganizationService organizationService)
        {
            _taskService = taskService;
            _userService = userService;
            _organizationService = organizationService;
        }

        public IActionResult Index(int id)
        {
            bool hasOrg = _organizationService.HasOrganization(id);
            if (!hasOrg) return NotFound();
            List<Models.Task> tasks = _taskService.GetAll(id);
            TaskViewModel viewModel = new TaskViewModel
            {
                Tasks = tasks,
                OrganizationId = id
            };
            return View(viewModel);
        }

        [Authorize(Roles = SystemRoles.OrganizationRole)]
        public IActionResult Create(int id)
        {
            bool hasOrg = _organizationService.HasOrganization(id);
            if (!hasOrg) return NotFound();
            IList<ApplicationUser> workerUsers = _userService.GetUsersByRoleAsync(SystemRoles.EmployeeRole.ToString()).GetAwaiter().GetResult();
            IList<ApplicationUser> organizationUsers = workerUsers.Where(u => u.OrganizationId == id).ToList();
            TaskViewModel viewModel = new TaskViewModel
            {
                EmployeeUsers = organizationUsers,
                OrganizationId = id
            };
            return View(viewModel);
        }

        [Authorize(Roles = SystemRoles.OrganizationRole)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TaskViewModel viewModel)
        {
            _taskService.CreateTask(viewModel);
            return RedirectToAction("Index", "Task", new { id = viewModel.OrganizationId });
        }
    }
}
