using System.Reflection;
using JSharp.Attributes;
using JSharp.Processors;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace JSharp;

public class JSharp : Task
{
    private static readonly HashSet<Assembly> Assemblies = new();
    
    // As these properties get filled in by the .csproj calling this task,
    // the CS8618 warning actually doesn't apply and can be suppressed.
#pragma warning disable CS8618
    [Required]
    public string AssemblyLocation { get; set; }
#pragma warning restore CS8618
    
    public override bool Execute()
    {
        // Compile list of assemblies
        var locations = AssemblyLocation.Split(';');
        foreach (var asm in locations)
        {
            try
            {
                Assemblies.Add(Assembly.LoadFile(asm));
            }
            catch (ArgumentException argumentException)
            {
                if (argumentException is ArgumentNullException)
                {
                    Log.LogError(subcategory: null,
                        errorCode: "JSHRP0001",
                        helpKeyword: null,
                        file: null,
                        lineNumber: 0,
                        columnNumber: 0,
                        endColumnNumber: 0,
                        endLineNumber: 0,
                        message: ErrorCodes.JSHRP0001);
                    return false;
                }
                Log.LogError(subcategory: null,
                    errorCode: "JSHRP0002",
                    helpKeyword: null,
                    file: null,
                    lineNumber: 0,
                    columnNumber: 0,
                    endColumnNumber: 0,
                    endLineNumber: 0,
                    message: ErrorCodes.JSHRP0002);
                return false;
            }
            catch (FileNotFoundException notFoundException)
            {
                Log.LogError(subcategory: null,
                    errorCode: "JSHRP0003",
                    helpKeyword: null,
                    file: null,
                    lineNumber: 0,
                    columnNumber: 0,
                    endColumnNumber: 0,
                    endLineNumber: 0,
                    message: ErrorCodes.JSHRP0003,
                    messageArgs: AssemblyLocation);
                return false;
            }
        }

        foreach (var type
                 in from assembly 
                     in Assemblies from type 
                     in assembly.DefinedTypes 
                 where type.CustomAttributes.Any(attr => attr.AttributeType != typeof(JExcludeAttribute))
                 select type)
        {
            var classBytes = new JClassProcessor(type).ClassBytes;
        }
        
        return true;
    }
}