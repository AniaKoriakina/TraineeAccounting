namespace TraineeAccounting.Domain.Entities;

public class Project
{
    public int ProjectId { get; set; }
    public string Name { get; set; }
    
    protected Project() { }

    public Project(string projectName)
    {
        Name = projectName ?? throw new ArgumentNullException(nameof(projectName));
    }

    public static bool IsUnique(string name, IEnumerable<Project> projects)
    {
        return !projects.Any(project => project.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}