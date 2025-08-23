using Microsoft.CodeAnalysis;

namespace WebServiceGenerator.GeneratorLibrary
{
    public class Property(IPropertySymbol symbol)
    {
        public string Name => symbol.Name;
        public string TypeName => symbol.Type.ToDisplayString();

        public bool HasGet = symbol.GetMethod != null;

        public bool HasSet = symbol.SetMethod != null;

        public override string ToString() => $"{TypeName} {Name} {{ {(HasGet ? "get; " : "")}{(HasSet ? "set;" : "")} }}";
    }
}
