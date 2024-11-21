namespace ClickUp.Data.Entities.MainEntities
{
    public class Project : BaseEntity
    {
        public string ProjectName { get; set; }
        public List<ProjectTable> Tables { get; set; }
    }
}
