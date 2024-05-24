using Bussiness.Service.Abstracts;
using Core.RepositoryAbstracts;
using HeroBiz.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HeroBiz.Controllers
{

    public class HomeController : Controller
    {
        private readonly ITeamService _teamService;

        public HomeController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public IActionResult Index()
        {
            var teams = _teamService.GetAllTeam();
            return View(teams);
        }

       
    }
}