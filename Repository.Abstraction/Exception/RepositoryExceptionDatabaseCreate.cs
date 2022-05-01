namespace Repository.Abstraction;

public class RepositoryExceptionDatabaseCreate : RepositoryExceptionDatabase
{
    private readonly static string Msg = "Could not create database";

    public RepositoryExceptionDatabaseCreate(string database)
        : base(Msg, database, Msg)
    {
    }

    public RepositoryExceptionDatabaseCreate(string database, Exception inner)
        : base(Msg, database, Msg, inner)
    {
    }
}
