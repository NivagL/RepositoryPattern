namespace Repository.Model
{
    public interface IConfigurationFilePolicy
    {
        IEnumerable<string> Files { get; }
    }
}
