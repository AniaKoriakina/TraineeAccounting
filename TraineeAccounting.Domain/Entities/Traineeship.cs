namespace TraineeAccounting.Domain.Entities;

public class Traineeship
{
    public int TraineeshipId { get; set; }
    public string Name { get; set; }
    
    protected Traineeship() { }

    public Traineeship(string traineeshipName)
    {
        Name = traineeshipName ?? throw new ArgumentNullException(nameof(traineeshipName));
    }
    
    public static bool IsUnique(string name, IEnumerable<Traineeship> traineeships)
    {
        return !traineeships.Any(traineeship => traineeship.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}