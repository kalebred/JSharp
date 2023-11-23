namespace JSharp.Class.Pool;

internal struct ConstantPoolValue
{
    public ConstantPoolType Type { get; set; }
    public object Value { get; set; }
    public int Index { get; set; }
    public int OwningIndex { get; set; }

    private bool Equals(ConstantPoolValue other)
    {
        return Type == other.Type && Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        return obj is ConstantPoolValue other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int) Type, Value);
    }
}