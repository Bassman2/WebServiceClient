using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace WebServiceGenerator.GeneratorLibrary
{
	public class Class(INamedTypeSymbol symbol)
	{
        public string Name => symbol.Name;

        public string NameSpace => symbol.ToDisplayString();

        public string NameWithNameSpace => symbol.ToDisplayString();

        public IEnumerable<Property> Properties => symbol.GetMembers().OfType<IPropertySymbol>().Select(p => new Property(p));
        
        public Property? GetProperty(string name) => Properties.FirstOrDefault(p => p.Name == name);
        
        public IEnumerable<Attribute> Attributes => symbol.GetAttributes().Select(a => new Attribute(a));
        
        public bool HasAttribute(string nameWithNamespace) => Attributes.Any(a => a.NameWithNameSpace == nameWithNamespace);
        
        public Attribute? GetAttribute(string nameWithNamespace) => Attributes.FirstOrDefault(a => a.NameWithNameSpace == nameWithNamespace);

        override public string ToString() => $"{NameSpace} - {Name} - {NameWithNameSpace}";

    }
}
