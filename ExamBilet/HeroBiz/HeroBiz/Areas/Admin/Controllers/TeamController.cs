using Bussiness.Exceptions;
using Bussiness.Service.Abstracts;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeroBiz.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class TeamController : Controller
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public IActionResult Index()
        {
           var teams=_teamService.GetAllTeam();
            return View(teams);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Team team)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                _teamService.AddTeam(team);
            }
            catch(FileContentException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
            }
            catch(FileSizeException ex)
            {
                ModelState.AddModelError(ex.PropertyName,ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int id)
        {
            var team=_teamService.GetTeam(x=>x.Id == id);
            if(team == null)
            {
                return NotFound();
            }
            _teamService.RemoveTeam(team.Id);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int id)
        {
            var oldTeam=_teamService.GetTeam(x=>x.Id==id);
            if(oldTeam == null)
            {
                return NotFound();
            }
           return View(oldTeam);
        }

        [HttpPost]
        public IActionResult Update(int id,Team team)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            _teamService.UpdateTeam(team.Id, team); 
            return RedirectToAction(nameof(Index));
        }
    }
}
