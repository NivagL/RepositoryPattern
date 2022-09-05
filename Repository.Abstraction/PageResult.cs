namespace Repository.Abstraction;

public class PageResult<TKey, TValue> 
{
    public int TotalItems { get; set; }
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public IDictionary<TKey, TValue> Data { get; set; }

    public PageResult(int totalItems, int pageNumber, int pageSize)
    {
#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
        Data = new Dictionary<TKey, TValue>();
#pragma warning restore CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
        TotalItems = totalItems;
        PageNumber = pageNumber;
        TotalPages = totalItems % pageSize > 0 ? totalItems / pageSize + 1 : totalItems / pageSize;
    }
}
