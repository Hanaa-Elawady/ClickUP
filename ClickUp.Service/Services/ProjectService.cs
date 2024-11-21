using ClickUp.Data.Entities.Default;
using ClickUp.Data.Entities.MainEntities;
using ClickUp.Repository.Interfaces;
using ClickUp.Service.Interfaces;

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

        public async Task DeleteProjectAsync(string projectId)
         => await _unitOfWork.GetRepository<Project>().DeleteAsync(projectId);

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
            => await _unitOfWork.GetRepository<Project>().GetAllAsync();

        public async Task<Project> GetProjectByIdAsync(string projectId)
        => await _unitOfWork.GetRepository<Project>().GetByIdAsync(projectId);

    }
}
