using ClickUp.Data.Entities.MainEntities;
using ClickUp.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClickUP.Web.Controllers
{
    public class ProjectController : BaseController
    {
        private readonly IProjectService service;

        public ProjectController(IProjectService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        => Ok(await service.GetAllProjectsAsync());

        [HttpPost]
        public async Task<ActionResult<Project>> CreateProject([FromBody] string projectName)
        => Ok(await service.CreateProjectAsync(projectName));

        [HttpGet]
        public Task<Project> GetProject(string projectId)
            => service.GetProjectByIdAsync(projectId);

    }
}
