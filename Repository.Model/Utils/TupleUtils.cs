namespace Repository.Utils;

public static class TupleUtils
{
    public static bool IsTuple(Type t)
    {
        try
        {
            var genericTypeDefinition = t.GetGenericTypeDefinition();
            var genericTypeName = genericTypeDefinition.FullName;

            if (t.IsGenericType && !string.IsNullOrEmpty(genericTypeName)
                && genericTypeName.StartsWith("System.Tuple"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch(Exception)
        {
            return false;
        }
    }

    public static bool IsTuple(object tuple)
    {
        Type t = tuple.GetType();
        return IsTuple(t);
    }

    public static string TupleTypeName(Type t)
    {
        var types = t.GetGenericArguments();
        return string.Join("_", types.Select(x => x.Name));
    }

    public static string ReverseTupleTypeName(Type t)
    {
        var types = t.GetGenericArguments().Reverse();
        return string.Join("_", types.Select(x => x.Name));
    }

    public static IEnumerable<object> TupleToEnumerable(object tuple)
    {
        var keys = new List<object>();
        if (IsTuple(tuple))
        {
            Type t = tuple.GetType();
            for (int i = 1; ; ++i)
            {
                var prop = t.GetProperty("Item" + i);

                if (prop == null)
                    break;
                    //yield break;

                //yield return prop.GetValue(tuple);
                var value = prop.GetValue(tuple);
                if(value != null)
                    keys.Add(value);
            }
        }
        return keys;
    }
}
