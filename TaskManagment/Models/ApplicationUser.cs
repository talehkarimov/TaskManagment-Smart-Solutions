using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagment.Models
{
    public class ApplicationUser : IdentityUser
    { 
        [StringLength(255)] 
        public string Name { get; set; }

        [StringLength(255)]
        public string Surname { get; set; }

        public bool IsDeleted { get; set; } = false;
        public bool HasAccessToLogin { get; set; } = true; 
        public DateTime? LastLoginDate { get; set; } 

        public ICollection<UserTask> UserTasks { get; set; }
        public Organization Organization { get; set; }
        public int? OrganizationId { get; set; }
    }
}
