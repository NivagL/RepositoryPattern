namespace Model.Serialiser;

public class LongSerialiser : ISerialiser<long>
{
    public char Delimiter { get; private set; }
    public LongSerialiser(char delimiter = ',')
        : this()
    {
        Delimiter = delimiter;
    }

    public Func<long, string> CreateString { get; set; }
    public Func<IEnumerable<long>, string> CreateStrings { get; set; }
    public Func<string, long> CreateObject { get; set; }
    public Func<string, IEnumerable<long>> CreateObjects { get; set; }

    public LongSerialiser()
    {
        CreateObject = x => long.Parse(x);
        CreateObjects = x => x.ToListEx<long>(Delimiter.ToString());
        CreateString = x => x.ToString();
        CreateStrings = x => string.Join(x.ToString(), Delimiter.ToString());
    }
}
