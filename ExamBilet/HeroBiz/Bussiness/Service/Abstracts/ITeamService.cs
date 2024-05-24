using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Service.Abstracts
{
    public  interface ITeamService
    {
        void AddTeam(Team team);
        void RemoveTeam(int id);
        void UpdateTeam(int id,Team team);
        Team GetTeam(Func<Team,bool>? func=null);
        List<Team> GetAllTeam(Func<Team,bool>? func=null);
    }
}
