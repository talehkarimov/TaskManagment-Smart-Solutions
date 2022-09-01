using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TaskManagment.Models;

namespace TaskManagment.ViewModel
{
    public class OrganizationViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Adress { get; set; }
    }
}
