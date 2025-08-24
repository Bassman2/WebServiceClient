﻿using Microsoft.CodeAnalysis;
using System.Linq;

namespace WebServiceGenerator.GeneratorLibrary
{
    public  class ConstructorArgument(TypedConstant arg) 
    {
        public string Value => FormatTypedConstant(arg);

        public string TypeName => arg.Type?.ToDisplayString() ?? string.Empty;

        public string Kind => arg.Kind.ToString();

        private string FormatTypedConstant(TypedConstant constant)
        {
            if (constant.IsNull)
            {
                return "null";
            }
            if (constant.Kind == TypedConstantKind.Array)
            {
                var values = constant.Values.Select(FormatTypedConstant);
                return $"[{string.Join(", ", values)}]";
            }
            if (constant.Type?.TypeKind == TypeKind.Enum)
            {
                var enumType = constant.Type;
                var value = constant.Value;
                if (value != null)
                {
                    var member = enumType.GetMembers()
                        .OfType<IFieldSymbol>()
                        .FirstOrDefault(f => f.HasConstantValue && Equals(f.ConstantValue, value));
                    if (member != null)
                        return $"{enumType.Name}.{member.Name}";
                }
                return $"{enumType.Name}.{value}";
            }
            if (constant.Type?.SpecialType == SpecialType.System_String)
            {
                return $"\"{constant.Value}\"";
            }
            return constant.Value?.ToString() ?? "null";
        }
    }
}
