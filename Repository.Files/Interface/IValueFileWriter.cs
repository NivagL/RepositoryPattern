namespace Repository.Files;

public interface IValueFileWriter<TKey, TValue>
{
    bool Write(TValue value, string subFolder = "");
    void WriteAll(IEnumerable<TValue> values, string subFolder = "");
}
