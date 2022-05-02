namespace Repository.Files;

public interface IFileWriter<TKey, TValue>
{
    TKey Write(TValue value, string subFolder = "");
    void WriteAll(IEnumerable<TValue> values, string subFolder = "");
}
