using System.Collections.Generic;
using TaskManagment.Models;
using System.ComponentModel.DataAnnotations;

namespace TaskManagment.ViewModel
{
    public class AccountViewModel
    {
        [DataType(DataType.Text)]
        public string Username { get; set; }
        public string Password { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        //Organization
        public string OrganizationName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        //Organization

        public string Name { get; set; }
        public string Surname { get; set; }
        public ApplicationUser User { get; set; }
        public List<UserTask> UserTasks { get; set; }
        public int? OrganizationId { get; set; }
        public List<Organization> Organizations { get; set; }
    }
}
