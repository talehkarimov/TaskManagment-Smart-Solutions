using System;
using System.Collections.Generic;
using TaskManagment.Models;
using System.ComponentModel.DataAnnotations;

namespace TaskManagment.ViewModel
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public string Status { get; set; }
        public int OrganizationId { get; set; }

        public List<TaskManagment.Models.Task> Tasks { get; set; }

        [Required]
        public List<string> UserIds { get; set; }
        public IList<ApplicationUser> EmployeeUsers { get; set; }
    }
}
