using ClickUp.Data.Entities.MainEntities;
using MongoDB.Bson;

namespace ClickUp.Service.Interfaces
{
    public interface IProjectService
    {
        public Task<Project> CreateProjectAsync(string projectName);
        public Task DeleteProjectAsync(ObjectId projectId);
        public Task<IEnumerable<Project>> GetAllProjectsAsync();
        public Task<Project> GetProjectByIdAsync(ObjectId projectId);
    }
}
