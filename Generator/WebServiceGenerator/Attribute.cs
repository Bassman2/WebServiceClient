using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace WebServiceGenerator
{

    public class Attribute(AttributeData data)
    {
        public string Name => data.AttributeClass?.Name ?? string.Empty;

        public string NameSpace => data.AttributeClass?.ContainingNamespace?.ToDisplayString() ?? string.Empty;
        
        public string NameWithNameSpace => data.AttributeClass?.ToDisplayString() ?? string.Empty;

        public IEnumerable<ConstructorArgument> ConstructorArguments
        {
            get
            {
                foreach (var arg in data.ConstructorArguments)
                {
                    yield return new ConstructorArgument(arg);
                }
            }
        }

        public IEnumerable<NamedArgument> NamedArguments
        {
            get
            {
                foreach (var arg in data.NamedArguments)
                {
                    yield return new NamedArgument(arg.Key, arg.Value);
                }
            }
        }

    }
}
