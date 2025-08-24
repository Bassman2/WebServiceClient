﻿using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace WebServiceGenerator.GeneratorLibrary
{

    public class Attribute(AttributeData data)
    {
        public string Name => data.AttributeClass?.Name ?? string.Empty;

        public string NameSpace => data.AttributeClass?.ContainingNamespace?.ToDisplayString() ?? string.Empty;
        
        public string FullName => data.AttributeClass?.ToDisplayString() ?? string.Empty;

        public IEnumerable<ConstructorArgument> ConstructorArguments => data.ConstructorArguments.Select(a => new ConstructorArgument(a));
       
        public IEnumerable<NamedArgument> NamedArguments => data.NamedArguments.Select(a => new NamedArgument(a.Key, a.Value));
    }
}
