namespace TraineeAccounting.Client.Response;

public class FormattedMessageError
{
    public string PropertyName { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
    public string AttemptedValue { get; set; } = string.Empty;
    public object? CustomState { get; set; }
    public int Severity { get; set; }
    public string ErrorCode { get; set; } = string.Empty;
    public Dictionary<string, object> FormattedMessagePlaceholderValues { get; set; } = new();
}