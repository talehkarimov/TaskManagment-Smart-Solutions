using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TaskManagment.Models;
using TaskManagment.Data;
using TaskManagment.Services.Abstract;
using TaskManagment.ViewModel;

namespace TaskManagment.Services.Concrete
{
    public class OrganizationService : IOrganizationService
    {
        private readonly AppDbContext _dbContext;

        public OrganizationService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool CreateOrganization(OrganizationViewModel viewModel)
        {
            var organization = new Organization
            {
                Name = viewModel.Name,
                PhoneNumber = viewModel.PhoneNumber,
                Address = viewModel.Adress
            };
            _dbContext.Organizations.Add(organization);
            int changes = _dbContext.SaveChanges();
            return changes > 0;
            
        }

        public List<Organization> GetAll()
        {
            return _dbContext.Organizations.AsNoTracking().ToList();
        }

        public Organization GetById(int id)
        {
            return _dbContext.Organizations.First(o => o.Id == id);
        }

        public bool HasOrganization(int id)
        {
            return _dbContext.Organizations.Any(o=>o.Id == id);
        }
    }
}
