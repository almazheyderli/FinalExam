using Bussiness.Exceptions;
using Bussiness.Service.Abstracts;
using Core.Models;
using Core.RepositoryAbstracts;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Service.Concretes
{
    public  class TeamService:ITeamService
    {

        private readonly ITeamRepository _teamRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TeamService(ITeamRepository teamRepository, IWebHostEnvironment webHostEnvironment)
        {
            _teamRepository = teamRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public void AddTeam(Team team)
        {
            if (team.ImgFile == null)
            {
                throw new ArgumentException("ImgFile", "not found");
            }
            if(team.ImgFile.Length >2097152)
            {

                throw new FileSizeException("ImgFile", "olcusu cox boyukdur");
            }
            if (!team.ImgFile.ContentType.Contains("image"))
            {
                throw new FileContentException("ImgFile", "sekil formatinda deyil");
            }
            string path=_webHostEnvironment.WebRootPath+@"/Upload/Service/"+ team.ImgFile.FileName;
            using(FileStream stream=new FileStream(path, FileMode.Create))
            {
                team.ImgFile.CopyTo(stream);
            }
            team.ImgUrl = team.ImgFile.FileName;
            _teamRepository.Add(team);
            _teamRepository.Commit();
        }

  

        public Team GetTeam(Func<Team, bool>? func = null)
        {
            return _teamRepository.Get(func);
        }

        public void RemoveTeam(int id)
        {
            var team=_teamRepository.Get(x=>x.Id==id);
            if (team == null)
            {
                throw new Exception();
            }
            string path = _webHostEnvironment.WebRootPath + @"/Upload/Service/" + team.ImgUrl;
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            _teamRepository.Remove(team);
            _teamRepository.Commit();
        }

       

        public void UpdateTeam(int id, Team team)
        {
           var oldTeam=_teamRepository.Get(x=>x.Id==id);
            if (oldTeam == null)
            {
                throw new NullReferenceException();
            }
            if (team.ImgFile != null)
            {
                string filename=team.ImgFile.FileName;
                string path = _webHostEnvironment.WebRootPath + @"/Upload/Service/" + team.ImgFile.FileName;
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    team.ImgFile.CopyTo(stream);
                }
               
                FileInfo fileInfo = new FileInfo(path+ oldTeam.ImgUrl);
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }
                oldTeam.ImgUrl = team.ImgFile.FileName;
            }
        
        oldTeam.Name= team.Name;
            oldTeam.Description= team.Description;
            _teamRepository.Commit();
        }

        List<Team> ITeamService.GetAllTeam(Func<Team, bool>? func)
        {
        return _teamRepository.GetAll(func);
        }
    }
}
