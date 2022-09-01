using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using TaskManagment.Data;
using TaskManagment.Models;
using TaskManagment.Services.Abstract;
using TaskManagment.ViewModel;

namespace TaskManagment.Services.Concrete
{
    public class TaskService: ITaskService
    {
        private readonly AppDbContext _dbContext;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IOrganizationService _organizationService;

        public TaskService(AppDbContext dbContext, IUserService userService,
            IEmailService emailService, IOrganizationService organizationService)
        {
            _dbContext = dbContext;
            _userService = userService;
            _emailService = emailService;
            _organizationService = organizationService;
        }

        public List<TaskManagment.Models.Task> GetAll(int id)
        {
            List<TaskManagment.Models.Task> tasks = _dbContext.Tasks.Where(t => t.OrganizationId == id).ToList();
            return tasks;
        }

        public bool CreateTask(TaskViewModel viewModel)
        {
            if (viewModel == null) return false;

            TaskManagment.Models.Task task = new TaskManagment.Models.Task
            {
                Title = viewModel.Title,
                Description = viewModel.Description,
                Deadline = viewModel.Deadline,
                TaskStatus = TaskManagment.ViewModel.TaskStatusEnum.TaskStatus.Appointed.ToString(),
                OrganizationId = viewModel.OrganizationId,
                UserTasks = new Collection<UserTask>()
            };

            Organization organization = _organizationService.GetById(viewModel.OrganizationId);

            string emailSubject = "Technical task";
            string emailBody = $"You have a new task by {organization.Name}";

            List<string> employeeUserIds = viewModel.UserIds;
            if (employeeUserIds != null)
            {
                foreach (var userId in employeeUserIds)
                {
                    ApplicationUser user = _userService.GetById(userId).GetAwaiter().GetResult();
                    UserTask userTask = new UserTask()
                    {
                        ApplicationUser = user
                    };
                    task.UserTasks.Add(userTask);
                    _emailService.SendEmail(user.Email, emailSubject, emailBody);
                }
            }
            _dbContext.Tasks.Add(task);
            _dbContext.SaveChanges();
            return true;
        }

        public List<UserTask> GetByUserId(string id)
        {
            List<UserTask> tasks = new List<UserTask>();
            if (id != null)
            {
                tasks = _dbContext.UserTasks.Include(ut => ut.Task).Where(ut => ut.ApplicationUserId == id).ToList();
            }
            return tasks;
        }
    }
}
