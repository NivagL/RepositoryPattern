namespace Model.Serialiser;

public class StringSerialiser : ISerialiser<string>
{
    public char Delimiter { get; private set; }
    public Func<string, string> CreateString { get; set; }
    public Func<IEnumerable<string>, string> CreateStrings { get; set; }
    public Func<string, string> CreateObject { get; set; }
    public Func<string, IEnumerable<string>> CreateObjects { get; set; }

    public StringSerialiser(char delimiter = '|')
    {
        Delimiter = delimiter;
        CreateString = x => x;
        CreateStrings = x => string.Join(Delimiter, x);
        CreateObject = x => x;
        CreateObjects = x => x.Split(Delimiter);
    }
}
