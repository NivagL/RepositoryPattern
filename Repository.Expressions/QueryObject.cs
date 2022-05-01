namespace Repository.Expressions
{
    /// <summary>
    /// Service interface query object
    /// </summary>
    public class QueryObject
    {
        /// <summary>
        /// Property name
        /// Uses a '.' notation for Parent.Child properties
        /// May use a '.' notation for children of a collection type
        /// </summary>
        public string PropertyName { get; set; } = string.Empty;
        /// <summary>
        /// The operator to apply to the name and value
        /// </summary>
        public string Operator { get; set; } = string.Empty;
        /// <summary>
        /// The value to match against the named property above
        /// Use '|' to seperate a list of values
        /// </summary>
        public object Value { get; set; } = new object();
    }
}
