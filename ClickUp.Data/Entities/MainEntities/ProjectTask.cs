using ClickUp.Data.Entities.IdentityEntities;

namespace ClickUp.Data.Entities.MainEntities
{
    public class ProjectTask : BaseEntity
    {
        public string TaskName { get; set; }
        public string Status { get; set; } = "ToDo";
        public string Assignee { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; } = "Normal";
    }
}