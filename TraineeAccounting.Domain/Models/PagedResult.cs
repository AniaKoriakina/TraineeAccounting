namespace TraineeAccounting.Domain.Models;

public class PagedResult<T>
{
    public List<T>? Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
    
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}