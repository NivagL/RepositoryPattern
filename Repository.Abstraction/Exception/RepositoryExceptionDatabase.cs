namespace Repository.Abstraction;

public class RepositoryExceptionDatabase : RepositoryException
{
    public string Database { get; set; }

    public RepositoryExceptionDatabase(string userMsg, string database, string msg)
        : base(userMsg, msg)
    {
        Database = database;
    }

    public RepositoryExceptionDatabase(string userMsg, string database, string msg, Exception inner)
        : base(userMsg, msg, inner)
    {
        Database = database;
    }
}
