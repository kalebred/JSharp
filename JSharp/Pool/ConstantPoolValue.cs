namespace JSharp.Pool;

public struct ConstantPoolValue
{
    public ConstantPoolType Type;
    public object Value;
    public int Index;

    public static explicit operator ConstantPoolValue(string s) => new() {Type = ConstantPoolType.String, Value = s};

    public bool Equals(ConstantPoolValue other)
    {
        return Type == other.Type && Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        return obj is ConstantPoolValue other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((int) Type * 397) ^ Value.GetHashCode();
        }
    }
}