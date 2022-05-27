using Repository.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Repository.Test.Model;

public class SimpleKeyTestModel 
    : IEqualityComparer<SimpleKeyTestModel>
    , IEquatable<SimpleKeyTestModel>
{
    public Guid Id { get; set; } = Guid.Empty;
    public DateTime Date { get; set; } = DateTime.MinValue;
    public bool Processed { get; set; } = false;
    public string Description { get; set; } = String.Empty;

    public bool Equals(SimpleKeyTestModel x, SimpleKeyTestModel y)
    {
        if (ReferenceEquals(x, null))
            return false;

        if (ReferenceEquals(y, null))
            return false;

        if (ReferenceEquals(x, y))
            return true;

        return x.Id == y.Id;
    }

    public bool Equals(SimpleKeyTestModel other)
    {
        if (ReferenceEquals(other, null))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return Id == other.Id;
    }

    public int GetHashCode([DisallowNull] SimpleKeyTestModel obj)
    {
        return obj.Id.GetHashCode();
    }

    public static bool operator ==(SimpleKeyTestModel x, SimpleKeyTestModel y)
    {
        if (ReferenceEquals(x, null))
            return false;

        if (ReferenceEquals(y, null))
            return false;

        if (ReferenceEquals(x, y))
            return true;

        return x.Id == y.Id;
    }

    public static bool operator !=(SimpleKeyTestModel x, SimpleKeyTestModel y)
    {
        if (ReferenceEquals(x, null))
            return true;

        if (ReferenceEquals(y, null))
            return true;

        if (ReferenceEquals(x, y))
            return false;

        return x.Id != y.Id;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(this, obj))
            return true;

        if (ReferenceEquals(obj, null))
            return false;

        var other = obj as SimpleKeyTestModel;
        if (other is null)
            return false;
        else
            return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
