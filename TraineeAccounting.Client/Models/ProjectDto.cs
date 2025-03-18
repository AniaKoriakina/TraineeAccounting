namespace TraineeAccounting.Client.Models;

public class ProjectDto: IHasId
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int TraineeCount { get; set; }
    public List<TraineeDto> Trainees { get; set; }
}