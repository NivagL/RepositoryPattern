namespace Repository.Abstraction;

public class RepositoryExceptionTimeout : RepositoryException
{
    public TimeSpan TimeOut;

    public RepositoryExceptionTimeout(string userMsg, string msg, TimeSpan timeOut)
        : base(userMsg, msg)
    {
        TimeOut = timeOut;
    }

    public RepositoryExceptionTimeout(string userMsg, string msg, Exception inner, TimeSpan timeOut)
        : base(userMsg, msg, inner)
    {
        TimeOut = timeOut;
    }
}
