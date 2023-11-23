using JSharp.Class.Attributes;

namespace JSharp.Class;

public struct MethodInfo
{
    public ushort AccessFlags { get; init; }
    public ushort NameIndex { get; init; }
    public ushort DescriptorIndex { get; init; }
    public ushort AttributesCount { get; init; }
    public AttributeInfo[] Attributes { get; init; }
}