namespace Repository.Files;

public interface IKeyFileWriter<TKey, TValue>
{
    TKey Write(TValue value, string subFolder = "");
}
