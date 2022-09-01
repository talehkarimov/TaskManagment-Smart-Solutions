using System.Collections.Generic; 

namespace TaskManagment.Models
{
    public class Organization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public ICollection<Task> Tasks { get; set; } 
    }
}
