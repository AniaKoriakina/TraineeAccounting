namespace TraineeAccounting.Application.Dtos;

public class SearchAndSortDto
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
    public string? Sort { get; set; } = "Name";
    public bool SortDirection { get; set; } = true;
}