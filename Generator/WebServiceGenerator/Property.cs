using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebServiceGenerator
{
    public class Property(IPropertySymbol symbol)
    {
        public string Name => symbol.Name;
        public string TypeName => symbol.Type.ToDisplayString();

        public bool HasGet = symbol.GetMethod != null;

        public bool HasSet = symbol.SetMethod != null;
    }
}
