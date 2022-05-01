namespace Repository.Abstraction;

public class RepositoryExceptionDatabaseDrop : RepositoryExceptionDatabase
{
    private readonly static string Msg = "Could not drop database";

    public RepositoryExceptionDatabaseDrop(string database)
        : base(Msg, database, Msg)
    {
    }

    public RepositoryExceptionDatabaseDrop(string database, Exception inner)
        : base(Msg, database, Msg, inner)
    {
    }
}
