using System.Text;

namespace Model.Serialiser;

internal static class StringListExtensions
{
    public static List<T> ToListEx<T>(this string x, string delimiter)
    {

        var split = x.Split(delimiter);
        if (typeof(T) == typeof(System.Guid))
        {
            var list = new List<Guid>();
            foreach (var item in split)
            {
                Guid converted = Guid.Parse(item);
                list.Add(converted);
            }
            return (List<T>)Convert.ChangeType(list, typeof(List<T>)); ;
        }
        else
        {
            var list = new List<T>();
            foreach (var item in split)
            {
                var converted = (T)Convert.ChangeType(item, typeof(T));
                list.Add(converted);
            }

            return list;
        }

        
    }

    public static List<T> ToListEx<T>(this string x, Func<string, T> stringToKey, string delimiter)
    {
        var list = new List<T>();
        var split = x.Split(delimiter);
        foreach (var item in split)
        {
            var converted = stringToKey(item);
            list.Add(converted);
        };
        return list;
    }

    public static string ToStingEx<T>(this List<T> x, Func<T, string> keyToString, string delimiter)
    {
        var value = new StringBuilder();
        foreach (var item in x)
        {
            if(value.Length > 0)
            {
                value.Append(delimiter);
            }
            var converted = keyToString(item);
            value.Append(converted);
        };
        return value.ToString();
    }

    public static string ToStingEx<T>(this IEnumerable<T> x, Func<T, string> keyToString, string delimiter)
    {
        var value = new StringBuilder();
        foreach (var item in x)
        {
            if (value.Length > 0)
            {
                value.Append(delimiter);
            }
            var converted = keyToString(item);
            value.Append(converted);
        };
        return value.ToString();
    }
}
