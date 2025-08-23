using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace WebServiceGenerator
{
	public class Class(INamedTypeSymbol symbol)
	{
        public string Name => symbol.Name;

        public string NameSpace => symbol.ToDisplayString();

        public string NameWithNameSpace => symbol.ToDisplayString();

        public IEnumerable<Property> Properties
        {
            get
            {
                foreach (var property in symbol.GetMembers().OfType<IPropertySymbol>())
                {
                    yield return new Property(property);
                }
            }
        }

        public Property? GetProperty(string name)
        {
            foreach (var property in Properties)
            {
                if (property.Name == name)
                {
                    return property;
                }
            }
            return null;
        }

        public IEnumerable<Attribute> Attributes
        {
            get
            {
                foreach (var attribute in symbol.GetAttributes())
                {
                    yield return new Attribute(attribute);
                }
            }
        }

        public bool HasAttribute(string nameWithNamespace)
        {
            foreach (var attribute in Attributes)
            {
                if (attribute.NameWithNameSpace == nameWithNamespace)
                {
                    return true;
                }
            }
            return false;
        }

        public Attribute? GetAttribute(string nameWithNamespace)
        {
            foreach (var attribute in Attributes)
            {
                if (attribute.NameWithNameSpace == nameWithNamespace)
                {
                    return attribute;
                }
            }
            return null;
        }   
    }
}
