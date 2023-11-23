using JSharp.Class;
using JSharp.Class.Attributes;
using JSharp.Class.Pool;

namespace JSharp.Attributes.Processors;

internal class JClassProcessor
{
     internal static byte[] GenerateBytecode(Type jClass)
     {
          var bytecode = new List<byte>
          {
               // Magic Number
               0xCA, 0xFE, 0xBA, 0xBE,
               // Minor Version
               0x00, 0x00,
               // Major Version
               0x41
          };

          // Constant Pool Count
          var constantPool = GenerateConstantPool(jClass);
          bytecode.Add((byte) (constantPool.Count + 1));

          // Constant Pool
          bytecode.AddRange(constantPool.GenerateBytecode());

          // Access Flags
          bytecode.Add(0x0001 | 0x0020);

          // This Class
          bytecode.Add(new[] { (byte) constantPool.GetIndex(new ConstantPoolValue
          {
               Type = ConstantPoolType.Class,
               Value = new ConstantPoolValue
               {
                    Type = ConstantPoolType.String,
                    Value = "Expected"
               }
          })});
          // Super Class
          bytecode.Add(new[] { (byte) constantPool.GetIndex(new ConstantPoolValue
          {
               Type = ConstantPoolType.Class,
               Value = new ConstantPoolValue
               {
                    Type = ConstantPoolType.String,
                    Value = "java/lang/Object"
               }
          }) });

          // Interfaces Count
          bytecode.Add(new byte[] { 0x00 });
          // Interfaces

          // Fields Count
          bytecode.Add(new byte[] { 0x00 });
          // Fields

          // Methods Count
          bytecode.Add(new byte[] {0x01});

          // Methods
          var initMethod = new MethodInfo
          {
               AccessFlags = 0x0001,
               NameIndex = (ushort) constantPool.GetIndex(new ConstantPoolValue
               {
                    Type = ConstantPoolType.String,
                    Value = "<init>"
               }),
               DescriptorIndex = (ushort) constantPool.GetIndex(new ConstantPoolValue
               {
                    Type = ConstantPoolType.String,
                    Value = "()V"
               }),
               AttributesCount = 1,
               Attributes = new[]
               {
                    new CodeAttributeInfo
                    {
                         NameIndex = (ushort) constantPool.GetIndex(new ConstantPoolValue
                         {
                              Type = ConstantPoolType.String,
                              Value = "Code"
                         }),
                         Length = 17,
                         MaxStack = 1,
                         MaxLocals = 1,
                         CodeLength = 5,
                         Code = new byte[]
                         {
                              0x2A, // aload_0
                              0xB7, 0x00, 0x01, // invokespecial #1
                              0xB1 // return
                         },
                         ExceptionTableLength = 0,
                         AttributesCount = 0
                    }
               }
          };


          // Attributes Count
          bytecode.Add(new byte[] { 0x00 });
          // Attributes

          return bytecode.ToArray();
     }

     private static ConstantPool GenerateConstantPool(Type jClass)
     {
          var constantPool = new ConstantPool();

          constantPool.Add(new ConstantPoolValue
          {
               Type = ConstantPoolType.MethodRef,
               Value = new ConstantPoolValue[]
               {
                    new()
                    {
                         Type = ConstantPoolType.Class,
                         Value = new ConstantPoolValue
                         {
                              Type = ConstantPoolType.String,
                              Value = "java/lang/Object"
                         }
                    },
                    new()
                    {
                         Type = ConstantPoolType.NameAndType,
                         Value = new ConstantPoolValue[]
                         {
                              new()
                              {
                                   Type = ConstantPoolType.String,
                                   Value = "<init>"
                              },
                              new()
                              {
                                   Type = ConstantPoolType.String,
                                   Value = "()V"
                              }
                         }
                    }
               }
          });
          constantPool.Add(new ConstantPoolValue
          {
               Type = ConstantPoolType.Class,
               Value = new ConstantPoolValue
               {
                    Type = ConstantPoolType.String,
                    Value = "Expected"
               }
          });
          constantPool.Add(new ConstantPoolValue
          {
               Type = ConstantPoolType.String,
               Value = "Code"
          });

          return constantPool;
     }
}