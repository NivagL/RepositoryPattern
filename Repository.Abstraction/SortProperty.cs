namespace Repository.Abstraction
{
    public class SortProperty
    {
        public string SortProperties { get; set; } = String.Empty;
        public SortOrderEnum SortOrder { get; set; } = SortOrderEnum.Ascending;
    }
}
