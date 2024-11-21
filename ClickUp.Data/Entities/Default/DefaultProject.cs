using ClickUp.Data.Entities.MainEntities;

namespace ClickUp.Data.Entities.Default
{
    public class DefaultProject :Project
    {
        public DefaultProject(string projectName)
        {
            ProjectName = projectName;
            Tables = new List<ProjectTable>
        {
            new ProjectTable { TableName = "ToDo", TableColor = "Blue" },
            new ProjectTable { TableName = "InProgress", TableColor = "Green" },
            new ProjectTable { TableName = "ReadyToTest", TableColor = "Yellow" },
            new ProjectTable { TableName = "Bugs", TableColor = "Red" },
            new ProjectTable { TableName = "Done", TableColor = "Orange" }
        };
        }
    }
}
