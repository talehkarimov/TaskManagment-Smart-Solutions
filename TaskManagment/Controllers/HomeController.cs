using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TaskManagment.Models;
using System.Diagnostics;
using TaskManagment.Models;
using TaskManagment.Services.Abstract;
using TaskManagment.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace TaskManagment.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IOrganizationService _organizationService;

        public HomeController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }   

        public IActionResult Index()
        {
            List<Organization> organizations = _organizationService.GetAll();
            HomeViewModel viewModel = new HomeViewModel()
            {
                Organizations = organizations
            };
            return View(viewModel);
        }
    }
}