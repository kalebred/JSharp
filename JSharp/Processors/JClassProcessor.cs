using JSharp.Pool;

namespace JSharp.Processors;

internal class JClassProcessor
{
    private readonly Type _type;
    private readonly ConstantPool _constantPool;

    public JClassProcessor(Type jClass)
    {
        _type = jClass;
        _constantPool = GenerateConstantPool();
    }

    internal IEnumerable<byte> GenerateBytecode()
     {
          var bytecode = new List<byte>
          {
               // Magic Number
               0xCA, 0xFE, 0xBA, 0xBE,
               // Minor Version
               0x00, 0x00,
               // Major Version
               0x41,
               // Constant Pool Count
               (byte) (_constantPool.Count + 1),
          };

          // Constant Pool
          bytecode.AddRange(_constantPool.GetBytes());

          // Access Flags

          // This Class

          // Super Class

          // Interfaces Count

          // Interfaces

          // Fields

          // Methods Count

          // Methods

          // Attributes Count

          // Attributes

          return bytecode.ToArray();
     }

     private ConstantPool GenerateConstantPool()
     {
         var constantPool = new ConstantPool(_type);
         return constantPool;
     }
}