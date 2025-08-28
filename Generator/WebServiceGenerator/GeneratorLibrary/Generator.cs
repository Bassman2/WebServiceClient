﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;


// https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.cookbook.md

// https://andrewlock.net/creating-a-source-generator-part-5-finding-a-type-declarations-namespace-and-type-hierarchy/

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

        public string AssemblyName => Compilation.AssemblyName ?? "";

        public string Namespace
        {
            get
            {
                //// Try to get the MSBuild RootNamespace property if available
                //if (Compilation.Options is Microsoft.CodeAnalysis.CSharp.CSharpCompilationOptions csharpOptions &&
                //    csharpOptions.SyntaxTreeOptionsProvider is { } provider &&
                //    Classes.Length > 0 &&
                //    provider.TryGetGlobalOption(Classes[0].SyntaxTree, "build_property.RootNamespace", out var ns) &&
                //    ns is string rootNs)
                //{
                //    return rootNs;
                //}

                // Fallback to assembly name
                return Compilation.AssemblyName ?? "";
            }
        }
        //=> Compilation.Assembly.ContainingNamespace.ToDisplayString();

        public IEnumerable<Class> GetAllClassesWithAttribute(string attributeFullName) => GetAllClasses().Where(c => c.HasAttribute(attributeFullName));

        public void AddSource(string hintName, string source) => Context.AddSource(hintName, source);

        public override IEnumerable<Attribute> Attributes => Compilation.Assembly.GetAttributes().Select(a => new Attribute(a));

        protected void CreateDebug()
        {
            StringBuilder sb = new();
            sb.AppendLine($"/*");
            sb.AppendLine();
            sb.AppendLine($"Global Namespace: {Namespace}");
            sb.AppendLine($"Assembly Name: {AssemblyName}");
            sb.AppendLine();

            //sb.AppendLine($"Global Namespace: {Compilation.GlobalNamespace.Name} - {Compilation.GlobalNamespace.ToDisplayString()} {(Compilation.GlobalNamespace.IsGlobalNamespace ? "global" : "")}");
            foreach (var x in Compilation.GlobalNamespace.GetNamespaceMembers())
            {
                sb.AppendLine($"Namespace: {x.Name} - {x.ToDisplayString()} {(x.IsGlobalNamespace ? "global" : "")} {x.NamespaceKind}");
            }
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
                        sb.AppendLine($"      {prop.Type.Name} {prop.Name} {{ {(prop.HasGet ? "get; " : "")}{(prop.HasSet ? "set;" : "")} }}");

                        DebugAttributes(sb, prop, 4);
                    }
                }

            }
            sb.AppendLine($"*/");
            AddSource($"Debug.g.cs", sb.ToString());
        }

        private void DebugAttributes(StringBuilder sb, BaseAttributes attributes, int indent)
        {
            string indentString = new(' ', indent * 2);
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
    
