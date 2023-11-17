namespace JSharp.Processors;

internal class JClassProcessor
{
    internal byte[] ClassBytes { get; private set; }
    
    internal JClassProcessor(Type type)
    {
        ClassBytes = GetClassBytes();
    }

    private byte[] GetClassBytes()
    {
        var bytecode = new List<byte>();
        
        // Magic Number
        bytecode.AddRange(new []{(byte) 0xCA, (byte) 0xFE, (byte) 0xBA, (byte) 0xBE});
        
        // Minor Version
        bytecode.Add(0x00);
        
        // Major Version
        bytecode.Add(0x41);
        
        // Constant Pool Count
        var constantPool = GenerateConstantPool();
        bytecode.Add((byte) constantPool.Length);
        
        // Constant Pool
        bytecode.AddRange(constantPool);
        
        // Access Flags
        bytecode.Add(0x0001 | 0x0020);
        
        // This Class Identifier
        // Super Class Identifier
        // Interface Count
        // Interface Table
        // Field Count
        // Field Table
        // Method Count
        // Method Table
        // Attribute Count
        // Attribute Table

        return bytecode.ToArray();
    }

    private byte[] GenerateConstantPool()
    {
        return new byte[] {};
    }
}