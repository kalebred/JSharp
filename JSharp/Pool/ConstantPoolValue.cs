namespace JSharp.Pool;

internal struct ConstantPoolValue
{
    public ConstantPoolType Type { get; init; }
    public object Value { get; init; }
    public int Index { get; set; }
    public int OwningIndex { get; set; }

    private bool Equals(ConstantPoolValue other) => Type == other.Type && Value.Equals(other.Value);

    public override bool Equals(object? obj) => obj is ConstantPoolValue other && Equals(other);

    public static explicit operator ConstantPoolValue(string s) => new() {Type = ConstantPoolType.String, Value = s};

    public override int GetHashCode() => HashCode.Combine((int) Type, Value);
}