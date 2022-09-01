using System.Collections.Generic;
using TaskManagment.Models;
using TaskManagment.ViewModel;

namespace TaskManagment.Services.Abstract
{
    public interface ITaskService
    {
        List<TaskManagment.Models.Task> GetAll(int id);
        bool CreateTask(TaskViewModel viewModel);
        List<UserTask> GetByUserId(string id);
    }
}
