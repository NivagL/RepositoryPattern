namespace Repository.Abstraction;

public class RepositoryException : Exception
{
    public readonly string UserMsg;

    public RepositoryException(string userMsg, string msg)
        : base(msg)
    {
        UserMsg = userMsg;
    }

    public RepositoryException(string userMsg, string msg, Exception inner)
        : base(msg, inner)
    {
        UserMsg = userMsg;
    }

    public override string Message => $"{UserMsg} {base.Message}";
}
