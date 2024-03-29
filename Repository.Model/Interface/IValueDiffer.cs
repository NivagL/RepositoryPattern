﻿namespace Model.Abstraction;

/// <summary>
/// Data difference, keys may be equal
/// </summary>
public interface IValueDiffer<TValue>
{
    Func<TValue, TValue, bool> Differ { get; }
}
