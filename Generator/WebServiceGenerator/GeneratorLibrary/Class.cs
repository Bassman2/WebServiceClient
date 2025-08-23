using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace WebServiceGenerator.GeneratorLibrary
{
	public class Class(INamedTypeSymbol symbol) : BaseAttributes
    {
        public string Name => symbol.Name;

        public string NameSpace => symbol.ToDisplayString();

        public string NameWithNameSpace => symbol.ToDisplayString();

        public IEnumerable<Property> Properties => symbol.GetMembers().OfType<IPropertySymbol>().Select(p => new Property(p));
        
        public Property? GetProperty(string name) => Properties.FirstOrDefault(p => p.Name == name);
        
        public override IEnumerable<Attribute> Attributes => symbol.GetAttributes().Select(a => new Attribute(a));

        override public string ToString() => $"{NameSpace} - {Name} - {NameWithNameSpace}";

    }
}
