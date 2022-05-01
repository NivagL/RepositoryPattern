namespace Repository.Abstraction;

public class RepositoryExceptionObjectNotFound : RepositoryException
{
    public RepositoryExceptionObjectNotFound(string userMsg, string msg)
        : base(userMsg, msg)
    {
    }

    public RepositoryExceptionObjectNotFound(string userMsg, string msg, Exception inner)
        : base(userMsg, msg, inner)
    {
    }
}
