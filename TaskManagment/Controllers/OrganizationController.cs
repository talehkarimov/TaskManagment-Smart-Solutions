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
    public class OrganizationController : Controller
    {
        private readonly IOrganizationService _organizationService;

        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }   
        
        [HttpGet]
        [Authorize(Roles = SystemRoles.Admin)]
        [Route("Add")]
        public IActionResult CreateOrganization(OrganizationViewModel viewModel)
        {
            return View(viewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrganization(OrganizationViewModel viewModel)
        {
            bool success = false;
            if (ModelState.IsValid)
            {
                success = _organizationService.CreateOrganization(viewModel);
            }
            TempData["Success"] = success;
            return RedirectToAction("Index", "Home");
        }
    }
}