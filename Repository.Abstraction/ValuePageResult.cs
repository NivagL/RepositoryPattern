namespace Repository.Abstraction;

public class ValuePageResult<TValue>
{
    public int TotalItems { get; set; }
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public IEnumerable<TValue> Data { get; set; }

    public ValuePageResult(int totalItems, int pageNumber, int pageSize)
    {
        Data = Enumerable.Empty<TValue>();
        TotalItems = totalItems;
        PageNumber = pageNumber;
        TotalPages = totalItems % pageSize > 0 ? totalItems / pageSize + 1 : totalItems / pageSize;
    }
}
