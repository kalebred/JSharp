namespace JSharp.Attributes;

[AttributeUsage(AttributeTargets.Class
                | AttributeTargets.Constructor
                | AttributeTargets.Enum
                | AttributeTargets.Field
                | AttributeTargets.Interface
                | AttributeTargets.Method
                | AttributeTargets.Property
                | AttributeTargets.Struct)]
public class JExcludeAttribute : Attribute { }