namespace Model.Serialiser;

public class GuidSerialiser : ISerialiser<Guid>
{
    public Func<Guid, string> CreateString { get; set; }
    public Func<IEnumerable<Guid>, string> CreateStrings { get; set; }
    public Func<string, Guid> CreateObject { get; set; }
    public Func<string, IEnumerable<Guid>> CreateObjects { get; set; }

    public GuidSerialiser()
    {
        CreateObject = x => Guid.Parse(x.Replace("\"",""));
        CreateObjects = x => GetGuidArray(x).ToListEx<Guid>(",");
        CreateString = x => string.Format("\"{0}\"", x.ToString());
        CreateStrings = x => string.Format("[{0}]",x.ToStingEx(GetGuidStr, ","));
    }

    private string GetGuidArray(string x)
    {
        return x.Replace("\"", "").Replace("[", "").Replace("]", "").Replace(Environment.NewLine, "").Replace(" ", "");
    }

    private string GetGuidStr(Guid x)
    {
        return string.Format("\"{0}\"", x.ToString());
    }
}
