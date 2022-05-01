namespace Repository.Abstraction;

public class RepositoryExceptionDatabaseTables : RepositoryExceptionDatabase
{
    private readonly static string Msg = "Could not create database tables";

    public RepositoryExceptionDatabaseTables(string database)
        : base(Msg, database, Msg)
    {
    }

    public RepositoryExceptionDatabaseTables(string database, Exception inner)
        : base(Msg, database, Msg, inner)
    {
    }
}
