namespace JSharp.Pool;

internal class ConstantPool
{
    private int _nextIndex;
    private readonly HashSet<ConstantPoolValue> _constantPool = new();

    internal ConstantPool(string qualifiedClassName, string qualifiedSuperClass = "java.lang.Object")
    {
        Add((ConstantPoolValue) qualifiedClassName.Replace('.', '/'));
    }

    internal ConstantPoolValue this[int index] => _constantPool.First(value => value.Index == index);

    internal void Add(ConstantPoolValue newValue)
    {
        // Constant Pool values can reference other values
        // These referenced values need to be added to the pool as well if they are not already
        var references = new HashSet<ConstantPoolValue>();
        switch (newValue.Value)
        {
            case ConstantPoolValue poolValue:
                references.Add(poolValue);
                break;
            case ConstantPoolValue[] poolValues:
            {
                foreach (var constantPoolValue in poolValues)
                {
                    references.Add(constantPoolValue);
                }

                break;
            }
        }

        // Loop through all nested values and attempt to add them
        foreach (var value in references.Where(value => !_constantPool.Contains(value)))
        {
            Add(value);
        }

        newValue.Index = _nextIndex++;
        _constantPool.Add(newValue);
    }
}