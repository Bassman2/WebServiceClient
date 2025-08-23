using Microsoft.CodeAnalysis;

namespace WebServiceGenerator.GeneratorLibrary
{
    public class NamedArgument(string name, TypedConstant arg) : ConstructorArgument(arg)
    {
        public string Name => name;

        public override string ToString() => $"Name: {Name} {base.ToString()}";
    }
}
