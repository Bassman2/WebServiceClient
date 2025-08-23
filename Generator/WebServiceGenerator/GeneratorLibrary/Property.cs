using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace WebServiceGenerator.GeneratorLibrary
{
    public class Property(IPropertySymbol symbol) : BaseAttributes
    {
        public string Name => symbol.Name;
        public string TypeName => symbol.Type.ToDisplayString();

        public bool HasGet = symbol.GetMethod != null;

        public bool HasSet = symbol.SetMethod != null;

        public override IEnumerable<Attribute> Attributes => symbol.GetAttributes().Select(a => new Attribute(a));
                
        public override string ToString() => $"{TypeName} {Name} {{ {(HasGet ? "get; " : "")}{(HasSet ? "set;" : "")} }}";
    }
}
