using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;


namespace ZeusInspector.Generators;


// Marcamos la clase para que Roslyn sepa que es un analizador de lenguaje C#
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class MiNodoValidator : DiagnosticAnalyzer
{
    // Definimos la regla de error/advertencia
    private static readonly DiagnosticDescriptor Rule = new(
        id: "ZEUS001",
        title: "La clase debe ser Sealed",
        messageFormat: "La clase '{0}' usa nuestro Addon pero no es 'partial'",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error, // Bloqueará la compilación si falla
        isEnabledByDefault: true
    );

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        // Configuración requerida por seguridad y rendimiento
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        // Le decimos a Roslyn que analice cada declaración de clase (NamedType)
        context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
    }

    private static void AnalyzeSymbol(SymbolAnalysisContext context)
    {
        var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

        // EJEMPLO: Si la clase hereda de Godot.Node (o tiene un atributo tuyo)
        // y no tiene la palabra clave 'partial'
        if (namedTypeSymbol.Name.StartsWith("MiNodoEspecial") && !namedTypeSymbol.IsSealed())
        {
            var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
            context.ReportDiagnostic(diagnostic);
        }
    }
}

// Extensión útil para verificar si es partial
public static class SymbolExtensions
{
    public static bool IsPartial(this INamedTypeSymbol symbol)
    {
        foreach (var syntaxRef in symbol.DeclaringSyntaxReferences)
        {
            if (syntaxRef.GetSyntax() is Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax typeDecl)
            {
                if (typeDecl.Modifiers.Any(m => m.IsKind(Microsoft.CodeAnalysis.CSharp.SyntaxKind.PartialKeyword)))
                    return true;
            }
        }
        return false;
    }

    public static bool IsSealed(this INamedTypeSymbol symbol)
    {
        foreach (var syntaxRef in symbol.DeclaringSyntaxReferences)
        {
            if (syntaxRef.GetSyntax() is Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax typeDecl)
            {
                if (typeDecl.Modifiers.Any(m => m.IsKind(Microsoft.CodeAnalysis.CSharp.SyntaxKind.SealedKeyword)))
                    return true;
            }
        }
        return false;
    }
}
