using System.Reflection;
using JSharp.Attributes;
using JSharp.Processors;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace JSharp;

public class JSharp : Task
{
    private readonly HashSet<Assembly> _assemblies = new();

    // As these properties get filled in by the .csproj calling this task,
    // the CS8618 warning actually doesn't apply and can be suppressed.
#pragma warning disable CS8618
    [Required] public string AssemblyLocation { get; set; }
#pragma warning restore CS8618

    public override bool Execute()
    {
        // Compile list of assemblies
        var locations = AssemblyLocation.Split(';');
        foreach (var asm in locations)
            try
            {
                Log.LogWarning($"Provided assembly '{asm}'");
                var asmBytes = File.ReadAllBytes(asm);
                _assemblies.Add(Assembly.Load(asmBytes));
            }
            catch (ArgumentException argumentException)
            {
                if (argumentException is ArgumentNullException)
                {
                    Log.LogError(null,
                                 "JSHRP0001",
                                 null,
                                 null,
                                 0,
                                 0,
                                 endColumnNumber: 0,
                                 endLineNumber: 0,
                                 message: ErrorCodes.JSHRP0001);
                    return false;
                }

                Log.LogError(null,
                             "JSHRP0002",
                             null,
                             null,
                             0,
                             0,
                             endColumnNumber: 0,
                             endLineNumber: 0,
                             message: ErrorCodes.JSHRP0002);
                return false;
            }
            catch (FileNotFoundException notFoundException)
            {
                Log.LogError(null,
                             "JSHRP0003",
                             null,
                             null,
                             0,
                             0,
                             endColumnNumber: 0,
                             endLineNumber: 0,
                             message: ErrorCodes.JSHRP0003,
                             messageArgs: AssemblyLocation);
                return false;
            }

        foreach (var type
                 in from assembly
                        in _assemblies
                    from type
                        in assembly.DefinedTypes
                    where type.CustomAttributes.Any(attr => attr.AttributeType != typeof(JExcludeAttribute))
                    select type)
        {
            var classBytes = new JClassProcessor(type).GenerateBytecode();
        }

        return true;
    }
}