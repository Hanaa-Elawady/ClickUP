using ClickUp.Data.Entities.IdentityEntities;
using ClickUp.Data.Entities.MainEntities;
using ClickUp.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ClickUp.Service.Hubs
{
    public class ClickUpHub :Hub
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClickUpHub(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HubMethodName("addTask")]
        public async Task AddTask(ObjectId ProjectId, ProjectTask task)
        {
            var project = await _unitOfWork.GetRepository<Project>().GetByIdAsync(ProjectId);
            if (project == null)
                throw new Exception($" project with id {ProjectId} not exist");

            var Table = project.Tables.FirstOrDefault(t => t.TableName == task.Status);

            if (Table == null)
                throw new Exception($" NO Table With This Name");

            Table.Tasks.Add(task);

            var filter = Builders<Project>.Filter.Eq(p => p.ProjectName, project.ProjectName);
            var update = Builders<Project>.Update.Set(p => p.Tables, project.Tables);
            await _unitOfWork.GetRepository<Project>().UpdateAsync(filter, update);

            await Clients.Group(project.ProjectName).SendAsync("newTask", task);

        }
    }
}
