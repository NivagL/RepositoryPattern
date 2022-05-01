namespace Repository.Serialiser;

public class IntSerialiser : ISerialiser<int>
{
    public char Delimiter { get; private set; }
    public IntSerialiser(char delimiter = ',') 
        : this()
    {
        Delimiter = delimiter;
    }

    public Func<int, string> CreateString { get; set; }
    public Func<IEnumerable<int>, string> CreateStrings { get; set; }
    public Func<string, int> CreateObject { get; set; }
    public Func<string, IEnumerable<int>> CreateObjects { get; set; }

    public IntSerialiser()
    {
        CreateObject = x => int.Parse(x);
        CreateObjects = x => x.ToListEx<int>(Delimiter.ToString());
        CreateString = x => x.ToString();
        CreateStrings = x => string.Join(x.ToString(), Delimiter.ToString());
    }
}
