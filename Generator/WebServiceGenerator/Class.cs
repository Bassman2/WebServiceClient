using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace WebServiceGenerator
{
	public class Class(INamedTypeSymbol symbol)
	{
        public static IEnumerable<Class> GetAllClasses((Compilation Left, ImmutableArray<ClassDeclarationSyntax> Right) tuple)
        {
            var (compilation, classes) = tuple;

            foreach (var cla in classes)
            {
                if (compilation.GetSemanticModel(cla.SyntaxTree).GetDeclaredSymbol(cla) is INamedTypeSymbol symbol)
                {
                    yield return new Class(symbol);
                }
            }
        }

        public static IEnumerable<Class> GetAllClassesWithAttribute((Compilation Left, ImmutableArray<ClassDeclarationSyntax> Right) tuple, string attribute)
        {
            foreach (var cla in Class.GetAllClasses(tuple))
            {
                if (cla.GetAttribute(attribute) != null)
                {
                    yield return cla;
                }
            }
        }

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
