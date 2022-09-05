namespace Repository.Abstraction;

/// <summary>
/// Defines a page for a query
/// </summary>
public class PageFilter
{
    /// <summary>
    /// The list of properties to sort on
    /// </summary>
    public string SortProperties { get; set; } = String.Empty;
    /// <summary>
    /// The delimeter used for the sort properties
    /// </summary>
    public char PropertyDelimeter { get; set; } = '|';
    /// <summary>
    /// Sort order
    /// </summary>
    public SortOrderEnum SortOrder { get; set; } = SortOrderEnum.Ascending;
    /// <summary>
    /// Which page to query
    /// </summary>
    public PageSelection PageSelection { get; set; } = new PageSelection()
    {
        PageSize = 50,
        PageNumber = 1
    };
}
