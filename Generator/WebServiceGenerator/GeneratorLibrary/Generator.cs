using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace WebServiceGenerator.GeneratorLibrary
{
    public class Generator : BaseAttributes, IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var provider = context.SyntaxProvider.CreateSyntaxProvider(
                    predicate: static (node, _) => node is ClassDeclarationSyntax,
                    transform: static (ctx, _) => (ClassDeclarationSyntax)ctx.Node
                    ).Where(m => m is not null);

            var compilation = context.CompilationProvider.Combine(provider.Collect());

            context.RegisterSourceOutput(compilation, Excecute);
        }

        private void Excecute(SourceProductionContext context, (Compilation Left, ImmutableArray<ClassDeclarationSyntax> Right) tuple)
        {
            Context = context;
            Compilation = tuple.Left;
            Classes = tuple.Right;
            try
            {
                Excecute();
            }
            catch (Exception ex)
            {
                var message = ex.ToString().Replace("/*", "/ *").Replace("*/", "* /");
                context.AddSource($"Error_{Guid.NewGuid():N}.g.cs", $"/*\r\n{message}\r\n*/");
            }
        }

        protected SourceProductionContext Context { get; private set; }

        protected Compilation Compilation { get; private set; } = null!;

        protected ImmutableArray<ClassDeclarationSyntax> Classes { get; private set; }

        public virtual void Excecute()
        { }

        public IEnumerable<Class> GetAllClasses()
        {
            foreach (var cla in Classes)
            {
                if (Compilation.GetSemanticModel(cla.SyntaxTree).GetDeclaredSymbol(cla) is INamedTypeSymbol symbol)
                {
                    yield return new Class(symbol);
                }
            }
        }

        public IEnumerable<Class> GetAllClassesWithAttribute(string attributeFullName) => GetAllClasses().Where(c => c.HasAttribute(attributeFullName));

        public void AddSource(string hintName, string source) => Context.AddSource(hintName, source);

        public override IEnumerable<Attribute> Attributes => Compilation.Assembly.GetAttributes().Select(a => new Attribute(a));

        protected void CreateDebug()
        {
            StringBuilder sb = new();
            sb.AppendLine($"/*");
            sb.AppendLine();

            // global attributes
            sb.AppendLine("Global Attributes");
            sb.AppendLine();
            DebugAttributes(sb, this, 1);
            sb.AppendLine();

            sb.AppendLine("Classes");
            foreach (var cl in GetAllClasses())
            {
                sb.AppendLine();
                sb.AppendLine( $"  Class: Name: {cl.Name}, Namespace: {cl.NameSpace}, FullName: {cl.FullName}");
                sb.AppendLine();

                if (cl.Properties.Any())
                {
                    DebugAttributes(sb, cl, 2);
                    sb.AppendLine($"    Properties:");
                    foreach (var prop in cl.Properties)
                    {
                        sb.AppendLine($"      {prop.TypeName} {prop.Name} {{ {(prop.HasGet ? "get; " : "")}{(prop.HasSet ? "set;" : "")} }}");

                        DebugAttributes(sb, prop, 4);
                    }
                }

            }
            sb.AppendLine($"*/");
            AddSource($"Debug.g.cs", sb.ToString());
        }

        private void DebugAttributes(StringBuilder sb, BaseAttributes attributes, int indent)
        {
            string indentString = new string(' ', indent * 2);
            foreach (var attr in attributes.Attributes)
            {
                sb.AppendLine($"{indentString}Attribute: Name: {attr.Name}, Namespace: {attr.NameSpace}, FullName {attr.FullName}");
                
                if (attr.ConstructorArguments.Any())
                {
                    sb.AppendLine($"{indentString}  Constructor Arguments");
                    foreach (var arg in attr.ConstructorArguments)
                    {
                        sb.AppendLine($"{indentString}    Value: {arg.Value}, Type: {arg.TypeName}, Kind: {arg.Kind}");
                    }
                }

                if (attr.NamedArguments.Any())
                {
                    sb.AppendLine($"{indentString}  Named Arguments");
                    foreach (var arg in attr.NamedArguments)
                    {
                        sb.AppendLine($"{indentString}    Name: {arg.Name}, Value: {arg.Value}, Type: {arg.TypeName}, Kind: {arg.Kind}");
                    }
                }
                sb.AppendLine();

            }
        }
    }
}
    
