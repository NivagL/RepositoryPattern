using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Repository.Test;

public class CompositeKeyTestModel
    : IEqualityComparer<CompositeKeyTestModel>
    , IEquatable<CompositeKeyTestModel>
{
    public Guid Id { get; set; } = Guid.Empty;
    public DateTime Date { get; set; } = DateTime.MinValue;
    public bool Processed { get; set; } = false;
    public string Description { get; set; } = String.Empty;

    public bool Equals(CompositeKeyTestModel x, CompositeKeyTestModel y)
    {
        if (ReferenceEquals(x, null))
            return false;

        if (ReferenceEquals(y, null))
            return false;

        if (ReferenceEquals(x, y))
            return true;

        return x.Id == y.Id
            && x.Date == y.Date;
    }

    public bool Equals(CompositeKeyTestModel other)
    {
        if (ReferenceEquals(other, null))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return Id == other.Id
            && Date == other.Date;
    }

    public int GetHashCode([DisallowNull] CompositeKeyTestModel obj)
    {
        var tuple = Tuple.Create(obj.Id, obj.Date);
        return tuple.GetHashCode();
    }

    public static bool operator ==(CompositeKeyTestModel x, CompositeKeyTestModel y)
    {
        if (ReferenceEquals(x, null))
            return false;

        if (ReferenceEquals(y, null))
            return false;

        if (ReferenceEquals(x, y))
            return true;

        return x.Id == y.Id
            && x.Date == y.Date;
    }

    public static bool operator !=(CompositeKeyTestModel x, CompositeKeyTestModel y)
    {
        if (ReferenceEquals(x, null))
            return true;

        if (ReferenceEquals(y, null))
            return true;

        if (ReferenceEquals(x, y))
            return false;

        return x.Id != y.Id
            || x.Date != y.Date;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(this, obj))
            return true;

        if (ReferenceEquals(obj, null))
            return false;

        var other = obj as CompositeKeyTestModel;
        if (other is null)
            return false;

        return Id == other.Id
            && Date == other.Date;
    }

    public override int GetHashCode()
    {
        var tuple = Tuple.Create(Id, Date);
        return tuple.GetHashCode();
    }
}
