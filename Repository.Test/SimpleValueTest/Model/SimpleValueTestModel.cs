using Repository.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Repository.Test;

public class SimpleValueTestModel 
    : IEqualityComparer<SimpleValueTestModel>
    , IEquatable<SimpleValueTestModel>
{
    public DateTime Date { get; set; } = DateTime.MinValue;
    public bool Processed { get; set; } = false;
    public string Description { get; set; } = String.Empty;

    public bool Equals(SimpleValueTestModel x, SimpleValueTestModel y)
    {
        if (ReferenceEquals(x, null))
            return false;

        if (ReferenceEquals(y, null))
            return false;

        if (ReferenceEquals(x, y))
            return true;

        return x.Date == y.Date
            && x.Processed == y.Processed
            && x.Description == y.Description;
    }

    public bool Equals(SimpleValueTestModel other)
    {
        if (ReferenceEquals(other, null))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return Date == other.Date
            && Processed == other.Processed
            && Description == other.Description;
    }

    public int GetHashCode([DisallowNull] SimpleValueTestModel obj)
    {
        var tuple = Tuple.Create(obj.Date, obj.Processed);
        return tuple.GetHashCode();
    }

    public static bool operator ==(SimpleValueTestModel x, SimpleValueTestModel y)
    {
        if (ReferenceEquals(x, null))
            return false;

        if (ReferenceEquals(y, null))
            return false;

        if (ReferenceEquals(x, y))
            return true;

        return x.Date == y.Date
            && x.Description == y.Description
            && x.Processed == y.Processed;
    }

    public static bool operator !=(SimpleValueTestModel x, SimpleValueTestModel y)
    {
        if (ReferenceEquals(x, null))
            return true;

        if (ReferenceEquals(y, null))
            return true;

        if (ReferenceEquals(x, y))
            return false;

        return x.Date != y.Date
            || x.Description != y.Description
            || x.Processed != y.Processed;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(this, obj))
            return true;

        if (ReferenceEquals(obj, null))
            return false;

        var other = obj as SimpleValueTestModel;
        if (other is null)
            return false;
        else
            return Date == other.Date
                && Description == other.Description
                && Processed == other.Processed;
    }

    public override int GetHashCode()
    {
        var tuple = Tuple.Create(Date, Processed);
        return tuple.GetHashCode();
    }
}
