using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebServiceGenerator
{
    public class NamedArgument(string name, TypedConstant arg) : ConstructorArgument(arg)
    {
        public string Name => name;

        public override string ToString() => $"Name: {Name} {base.ToString()}";
    }
}
