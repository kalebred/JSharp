using JSharp.Attributes;
using JSharp.Attributes.Processors;

namespace JSharp.Example;

public class Expected
{
    public static void Main(string[] args)
    {
        foreach (var line in JClassProcessor.GenerateBytecode(typeof(Expected)))
        {
            Console.WriteLine(BitConverter.ToString(line).Replace("-", " "));
        }
    }
}