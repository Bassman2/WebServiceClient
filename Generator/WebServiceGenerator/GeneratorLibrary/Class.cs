﻿using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace WebServiceGenerator.GeneratorLibrary
{
	public class Class(INamedTypeSymbol symbol) : BaseAttributes
    {
        public string Name => symbol.Name;

        public string NameSpace => symbol.ToDisplayString();

        public string RootNameSpace
        {
            get
            {
                //string nameSpace = NameSpace;
                //int dotIndex = nameSpace.IndexOf('.');
                //if (dotIndex > 0)
                //{
                //    nameSpace = nameSpace.Substring(0, dotIndex);
                //}
                //return nameSpace;


                var ns = NameSpace;
                if (string.IsNullOrEmpty(ns))
                    return string.Empty;

                int dotIndex = ns.IndexOf('.');
                return dotIndex > 0 ? ns.Substring(0, dotIndex) : ns;
            }
        }
        
        public string FullName => symbol.ToDisplayString();

        public IEnumerable<Property> Properties => symbol.GetMembers().OfType<IPropertySymbol>().Select(p => new Property(p));
        
        public Property? GetProperty(string name) => Properties.FirstOrDefault(p => p.Name == name);
        
        public override IEnumerable<Attribute> Attributes => symbol.GetAttributes().Select(a => new Attribute(a));
    }
}
