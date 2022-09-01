using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagment.Models
{
    public sealed class SystemRoles
    {
        public const string Admin = "Admin";
        public const string OrganizationRole = "Organization";
        public const string EmployeeRole = "Employee";


        public static readonly IList<string> AllRoles = new List<string>
        {
            Admin, OrganizationRole, EmployeeRole
        };

        public const string AdminOrOrganization = Admin + ", " + OrganizationRole;
    }
}
