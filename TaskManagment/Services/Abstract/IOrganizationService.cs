using System.Collections.Generic;
using TaskManagment.Models;
using TaskManagment.ViewModel;

namespace TaskManagment.Services.Abstract
{
    public interface IOrganizationService
    {
        bool HasOrganization(int id);
        List<Organization> GetAll();
        Organization GetById(int id);
        bool CreateOrganization(OrganizationViewModel viewModel);
    }
}
