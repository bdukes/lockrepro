using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MyExample.ThisIsASourceGenerator;

[Generator]
public class DataContextEnumConventionGenerator : IIncrementalGenerator
{
    private const string DataContextFullTypeName = $"{DataContextNamespace}.{DataContextTypeName}";
    private const string DataContextNamespace = "MyExample.Data";
    private const string DataContextTypeName = "TheDataContext";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var dataContextProvider = context.SyntaxProvider.CreateSyntaxProvider(predicate: HasExpectedName, transform: GetClass);
        context.RegisterSourceOutput(context.CompilationProvider.Combine(dataContextProvider.Collect()), Execute);
    }

    private static void AddEnumTypes(SortedSet<INamedTypeSymbol> enumTypes, INamedTypeSymbol entityType)
    {
        foreach (var entityMember in entityType.GetMembers())
        {
            if (entityMember is IPropertySymbol { Kind: SymbolKind.Property, Type: INamedTypeSymbol namedType, })
            {
                if (namedType.TypeKind == TypeKind.Enum)
                {
                    enumTypes.Add(namedType);
                }
                else if (namedType.IsGenericType
                         && namedType.TypeArguments[0] is INamedTypeSymbol { TypeKind: TypeKind.Enum, } enumType)
                {
                    enumTypes.Add(enumType);
                }
            }
        }

        if (entityType.BaseType is not null)
        {
            AddEnumTypes(enumTypes, entityType.BaseType);
        }
    }

    private static void Execute(SourceProductionContext context, (Compilation Compilation, ImmutableArray<ClassDeclarationSyntax> Classes) value)
    {
        var (compilation, classes) = value;
        if (!classes.IsDefaultOrEmpty)
        {
            var dataContextType = compilation.GetTypeByMetadataName(DataContextFullTypeName);
            if (dataContextType is not null)
            {
                GenerateEnumConfiguration(context, dataContextType);
            }
            else
            {
                ReportMissingDataContextType(context);
            }
        }
        else
        {
            ReportMissingDataContextType(context);
        }
    }

    private static void GenerateEnumConfiguration(SourceProductionContext context, INamedTypeSymbol dataContextType)
    {
        var enumTypes = new SortedSet<INamedTypeSymbol>(new SymbolComparer());
        foreach (var member in dataContextType.GetMembers())
        {
            context.CancellationToken.ThrowIfCancellationRequested();
            if (member is IPropertySymbol { Kind: SymbolKind.Property, Type.Name: "DbSet", } dbSetProperty
                && dbSetProperty.Type is INamedTypeSymbol dbSetType
                && dbSetType.TypeArguments[0] is INamedTypeSymbol entityType)
            {
                AddEnumTypes(enumTypes, entityType);
            }
        }

        var sourceBuilder = new StringBuilder();
        sourceBuilder.AppendLine(
            $$"""
              namespace {{DataContextNamespace}};

              using Microsoft.EntityFrameworkCore;

              partial class {{DataContextTypeName}}
              {
                  static partial void ConfigureEnumTypes(ModelConfigurationBuilder modelConfigurationBuilder)
                  {
              """);

        foreach (var enumType in enumTypes)
        {
            var maxLength = 0;
            foreach (var enumMember in enumType.GetMembers())
            {
                if (enumMember is IFieldSymbol field)
                {
                    maxLength = Math.Max(maxLength, field.Name.Length);
                }
            }

            var enumName = enumType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            sourceBuilder.AppendLine($$"""
                                               ConfigureEnumType<{{enumName}}>(modelConfigurationBuilder, {{maxLength}});
                                       """);
        }

        sourceBuilder.AppendLine("""
                                     }
                                 }
                                 """);

        context.AddSource($"{DataContextTypeName}.g.cs", sourceBuilder.ToString());
    }

    private static ClassDeclarationSyntax GetClass(GeneratorSyntaxContext context, CancellationToken token)
    {
        return (ClassDeclarationSyntax)context.Node;
    }

    private static bool HasExpectedName(SyntaxNode node, CancellationToken token)
    {
        return node is ClassDeclarationSyntax { Identifier.ValueText: DataContextTypeName, };
    }

    private static void ReportMissingDataContextType(SourceProductionContext context)
    {
        var descriptor = new DiagnosticDescriptor(
            "CCI2000",
            $"Expected {DataContextFullTypeName} to be defined",
            $"Expected {DataContextFullTypeName} to be defined",
            "Data",
            DiagnosticSeverity.Warning,
            true);
        context.ReportDiagnostic(Diagnostic.Create(descriptor, null));
    }

    private class SymbolComparer : IComparer<INamedTypeSymbol>
    {
        public int Compare(INamedTypeSymbol? x, INamedTypeSymbol? y)
        {
            return string.Compare(x?.Name, y?.Name, StringComparison.Ordinal);
        }
    }
}