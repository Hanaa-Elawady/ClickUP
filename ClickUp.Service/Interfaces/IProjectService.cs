using ClickUp.Data.Entities.MainEntities;

namespace ClickUp.Service.Interfaces
{
    public interface IProjectService
    {
        public Task<Project> CreateProjectAsync(string projectName);
        public Task DeleteProjectAsync(string projectId);
        public Task<IEnumerable<Project>> GetAllProjectsAsync();
        public Task<Project> GetProjectByIdAsync(string projectId);
    }
}
