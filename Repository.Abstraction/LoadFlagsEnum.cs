namespace Repository.Abstraction;

[Flags]
public enum LoadFlagsEnum
{
    None = 1,
    All = 2,
    Reference = 4,
    Parent = 8,
    Children = 16,
    State = 32,
    Aggregate = 64,
    Output = 128
}
