using System.Text;

namespace JSharp.Class.Pool;

internal class ConstantPool
{
    /// <summary>
    /// HashSet containing all the values in the constant pool.
    /// </summary>
    private readonly HashSet<ConstantPoolValue> _values = new();

    /// <summary>
    /// The next index that will be assigned to any new values added to the constant pool.
    /// </summary>
    private int _nextIndex => _values.Count + 1;

    /// <summary>
    /// The length of the constant pool.
    /// </summary>
    public int Count => _values.Count;

    /// <summary>
    /// Add a new value to the constant pool. All references will be automatically added if they do not already exist
    /// in the pool.
    /// </summary>
    /// <param name="newValue">The value being added to the pool</param>
    /// <exception cref="ArgumentOutOfRangeException">newValue.Type is not a valid <see cref="ConstantPoolType"/></exception>
    public void Add(ConstantPoolValue newValue, int owningIndex = -1)
    {
        // Give the current pool value the next available index
        var index = _nextIndex;

        // Validate the value being added to the pool.
        // Currently unsupported: MethodHandle, Dynamic, InvokeDynamic
        // i fucking love object boxing :D
        switch (newValue.Type)
        {
            case ConstantPoolType.String:
                if (newValue.Value is not string) return;
                break;
            case ConstantPoolType.Integer:
                if (newValue.Value is not int) return;
                break;
            case ConstantPoolType.Float:
                if (newValue.Value is not float) return;
                break;
            case ConstantPoolType.Long:
                if (newValue.Value is not long) return;
                break;
            case ConstantPoolType.Double:
                if (newValue.Value is not double) return;
                break;
            case ConstantPoolType.Class:
            case ConstantPoolType.StringRef:
            case ConstantPoolType.MethodType:
            case ConstantPoolType.Module:
            case ConstantPoolType.Package:
                if (newValue.Value is not ConstantPoolValue {Type: ConstantPoolType.String}) return;
                break;
            case ConstantPoolType.FieldRef:
            case ConstantPoolType.MethodRef:
            case ConstantPoolType.InterfaceMethodRef:
                if (newValue.Value is not (ConstantPoolValue[] and
                    [{Type: ConstantPoolType.Class}, {Type: ConstantPoolType.NameAndType}])) return;
                break;
            case ConstantPoolType.NameAndType:
                if (newValue.Value is not (ConstantPoolValue[] and
                    [{Type: ConstantPoolType.String}, {Type: ConstantPoolType.String}])) return;
                break;
            case ConstantPoolType.MethodHandle:
                // TODO: Implement MethodHandle validation
                break;
            case ConstantPoolType.Dynamic:
                // TODO: Implement Dynamic validation
                break;
            case ConstantPoolType.InvokeDynamic:
                // TODO: Implement InvokeDynamic validation
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newValue));
        }

        // Add any nested ConstantPoolValues to the pool if they do not already exist.
        switch (newValue.Value)
        {
            case ConstantPoolValue constantPoolValue:
            {
                if (_values.Contains(constantPoolValue)) break;
                Add(constantPoolValue, index);
                break;
            }
            case ConstantPoolValue[] constantPoolValues:
            {
                foreach (var value in constantPoolValues)
                {
                    if (_values.Contains(value)) continue;
                    Add(value, index);
                }
                break;
            }
        }

        // Add the value to the pool if it is not already added.
        if (_values.Contains(newValue)) return;
        newValue.Index = index;
        newValue.OwningIndex = owningIndex;
        _values.Add(newValue);
    }

    /// <summary>
    /// Get the index of a given <see cref="ConstantPoolValue"/>
    /// </summary>
    /// <param name="value">A <see cref="ConstantPoolValue"/> to find the index of</param>
    /// <returns>Index of the <see cref="ConstantPoolValue"/>, -1 if not in pool</returns>
    public int GetIndex(ConstantPoolValue value)
    {
        return _values.Contains(value) ? value.Index : -1;
    }

    /// <summary>
    /// Generate the Java bytecode representing this pool
    /// </summary>
    /// <returns>A byte[] containing the bytes representing this constant pool</returns>
    public byte[] GenerateBytecode()
    {
        var bytecode = new List<byte>();
        foreach (var value in _values.OrderBy(value => value.Index))
        {
            bytecode.Add((byte) value.Type);
            switch (value.Type)
            {
                case ConstantPoolType.String:
                    var stringValue = value.Value as string;
                    var stringBytes = Encoding.UTF8.GetBytes(stringValue!).ToArray();
                    bytecode.Add((byte) stringBytes.Length);
                    bytecode.AddRange(stringBytes);
                    break;
                case ConstantPoolType.Integer:
                    bytecode.AddRange(BitConverter.GetBytes((int) value.Value).Reverse());
                    break;
                case ConstantPoolType.Float:
                    bytecode.AddRange(BitConverter.GetBytes((float) value.Value).Reverse());
                    break;
                case ConstantPoolType.Long:
                    bytecode.AddRange(BitConverter.GetBytes((long) value.Value).Reverse());
                    break;
                case ConstantPoolType.Double:
                    bytecode.AddRange(BitConverter.GetBytes((double) value.Value).Reverse());
                    break;
                case ConstantPoolType.Class:
                    bytecode.AddRange(BitConverter.GetBytes(((ConstantPoolValue) value.Value).Index).Reverse());
                    break;
                case ConstantPoolType.StringRef:
                    bytecode.AddRange(BitConverter.GetBytes(((ConstantPoolValue) value.Value).Index).Reverse());
                    break;
                case ConstantPoolType.FieldRef:
                    var fieldRefs = (ConstantPoolValue[]) value.Value;
                    bytecode.AddRange(BitConverter.GetBytes(fieldRefs[0].Index).Reverse());
                    bytecode.AddRange(BitConverter.GetBytes(fieldRefs[1].Index).Reverse());
                    break;
                case ConstantPoolType.MethodRef:
                    var methodRefs = (ConstantPoolValue[]) value.Value;
                    bytecode.AddRange(BitConverter.GetBytes(methodRefs[0].Index).Reverse());
                    bytecode.AddRange(BitConverter.GetBytes(methodRefs[1].Index).Reverse());
                    break;
                case ConstantPoolType.InterfaceMethodRef:
                    var interfaceMethodRefs = (ConstantPoolValue[]) value.Value;
                    bytecode.AddRange(BitConverter.GetBytes(interfaceMethodRefs[0].Index).Reverse());
                    bytecode.AddRange(BitConverter.GetBytes(interfaceMethodRefs[1].Index).Reverse());
                    break;
                case ConstantPoolType.NameAndType:
                    var nameAndTypeRefs = (ConstantPoolValue[]) value.Value;
                    bytecode.AddRange(BitConverter.GetBytes(nameAndTypeRefs[0].Index).Reverse());
                    bytecode.AddRange(BitConverter.GetBytes(nameAndTypeRefs[1].Index).Reverse());
                    break;
                case ConstantPoolType.MethodHandle:
                    break;
                case ConstantPoolType.MethodType:
                    bytecode.AddRange(BitConverter.GetBytes(((ConstantPoolValue) value.Value).Index).Reverse());
                    break;
                case ConstantPoolType.Dynamic:
                    break;
                case ConstantPoolType.InvokeDynamic:
                    break;
                case ConstantPoolType.Module:
                    bytecode.AddRange(BitConverter.GetBytes(((ConstantPoolValue) value.Value).Index).Reverse());
                    break;
                case ConstantPoolType.Package:
                    bytecode.AddRange(BitConverter.GetBytes(((ConstantPoolValue) value.Value).Index).Reverse());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        return bytecode.ToArray();
    }
}