namespace JSharp.Class.Attributes;

public class CodeAttributeInfo : AttributeInfo
{
    public ushort MaxStack { get; init; }
    public ushort MaxLocals { get; init; }
    public uint CodeLength { get; init; }
    public byte[] Code { get; init; }
    public ushort ExceptionTableLength { get; init; }

    public ushort AttributesCount { get; init; }
    public AttributeInfo[] Attributes { get; init; }
}