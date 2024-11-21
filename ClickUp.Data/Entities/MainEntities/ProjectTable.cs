namespace ClickUp.Data.Entities.MainEntities
{
    public class ProjectTable : BaseEntity
    {
        public string TableName { get; set; }
        public string TableColor { get; set; }
        public List<ProjectTask> Tasks { get; set; }
    }
}