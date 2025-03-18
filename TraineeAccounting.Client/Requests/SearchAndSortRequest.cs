namespace TraineeAccounting.Client.Requests;

public class SearchAndSortRequest
{
    public string Search { get; set; } = string.Empty;
    public string Sort { get; set; } = "Id";
    public bool SortDirection { get; set; } = true;
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}