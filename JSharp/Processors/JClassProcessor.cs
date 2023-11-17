namespace JSharp.Processors;

internal class JClassProcessor
{
    internal JClassProcessor(Type type)
    {
        
    }
    
    internal byte[][] GetClassBytes()
    {
        var bytecode = new HashSet<byte[]>();
        
        // Magic Number
        bytecode.Add(new[] {(byte) 0xCA, (byte) 0xFE, (byte) 0xBA, (byte) 0xBE});
        
        // Minor Version
        bytecode.Add(new[] {(byte) 0x00});
        
        // Major Version
        bytecode.Add(new[] {(byte) 0x41});
        
        // Constant Pool Count
        var constantPool = GenerateConstantPool();
        bytecode.Add(new[] {(byte) constantPool.Length});
        
        // Constant Pool
        bytecode.Add(constantPool);
        
        // Access Flags
        bytecode.Add(new[] {(byte) (0x0001 | 0x0020)});
        
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