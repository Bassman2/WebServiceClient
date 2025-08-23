using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace WebServiceGenerator
{
    public class Generator : IIncrementalGenerator
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
            Excecute();
        }

        protected SourceProductionContext Context { get; private set; }

        protected Compilation? Compilation { get; private set; }

        protected ImmutableArray<ClassDeclarationSyntax> Classes { get; private set; }

        public virtual void Excecute()
        { }

        public IEnumerable<Class> GetAllClasses()
        {
            foreach (var cla in Classes)
            {
                if (Compilation!.GetSemanticModel(cla.SyntaxTree).GetDeclaredSymbol(cla) is INamedTypeSymbol symbol)
                {
                    yield return new Class(symbol);
                }
            }
        }

        public IEnumerable<Class> GetAllClassesWithAttribute(string attributeFullName)
        {
            foreach (var cla in GetAllClasses())
            {
                if (cla.GetAttribute(attributeFullName) != null)
                {
                    yield return cla;
                }
            }
        }

        public void AddSource (string hintName, string source) => Context.AddSource(hintName, source);
    }
}
