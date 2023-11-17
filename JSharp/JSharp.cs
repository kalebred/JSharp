using System.Reflection;
using JSharp.Attributes;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace JSharp;

public class JSharp : Task
{
    private static readonly HashSet<Assembly> Assemblies = new();
    
    [Required]
    public string AssemblyLocation { get; set; }
    
    public override bool Execute()
    {
        // Compile list of assemblies
        var assemblies = AssemblyLocation.Split(';');
        foreach (var asm in assemblies)
        {
            try
            {
                Assemblies.Add(Assembly.LoadFile(asm));
            }
            catch (ArgumentException argumentException)
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
            catch (FileNotFoundException notFoundException)
            {
                Log.LogError(subcategory: null,
                    errorCode: "JSHRP0002",
                    helpKeyword: null,
                    file: null,
                    lineNumber: 0,
                    columnNumber: 0,
                    endColumnNumber: 0,
                    endLineNumber: 0,
                    message: ErrorCodes.JSHRP0002,
                    messageArgs: AssemblyLocation);
            }
        }
        
        return true;
    }
}