using ClickUp.Data.Entities.Default;
using ClickUp.Data.Entities.MainEntities;
using ClickUp.Repository.Interfaces;
using ClickUp.Service.Interfaces;
using MongoDB.Bson;

namespace ClickUp.Service.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Project> CreateProjectAsync(string projectName)
        {
            if (projectName is null)
                throw new ArgumentNullException(nameof(projectName));

            var newProject = new DefaultProject(projectName);
            await _unitOfWork.GetRepository<Project>().AddAsync(newProject);
            return newProject;
        }

        public async Task DeleteProjectAsync(ObjectId projectId)
         => await _unitOfWork.GetRepository<Project>().DeleteAsync(projectId);

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
            => await _unitOfWork.GetRepository<Project>().GetAllAsync();

        public async Task<Project> GetProjectByIdAsync(ObjectId projectId)
        => await _unitOfWork.GetRepository<Project>().GetByIdAsync(projectId);

    }
}
