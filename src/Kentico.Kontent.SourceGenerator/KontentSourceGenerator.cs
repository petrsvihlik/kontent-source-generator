using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.ModelGenerator.Core;
using Kentico.Kontent.ModelGenerator.Core.Configuration;
using Kentico.Kontent.SourceGenerator.Fakes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Options;

namespace Kentico.Kontent.SourceGenerator
{
    [Generator]
    public class KontentSourceGenerator : ISourceGenerator
    {
        private const string attributeText = @"
using System;
namespace KenticoKontentModels
{
    [System.AttributeUsage(System.AttributeTargets.Assembly, AllowMultiple=true)]
    sealed class KontentModelsAttribute : Attribute
    {
        public string ProjectId { get; set; }

        public KontentModelsAttribute(string projectId)
        {
            ProjectId = projectId;
        }
    }
}
";

        public void Initialize(InitializationContext context)
        {
            // No init required
        }

        public void Execute(SourceGeneratorContext context)
        {
            try
            {
                ExecuteInternal(context);
            }
            catch (Exception ex)
            {
                string RLB(string s)
                {
                    return s?.Replace("\n", "").Replace("\r", "");
                }
                context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor("KSG001",
                    "SourceGeneratorError",
                    $"MSG: {RLB(ex.Message)} Stack: {RLB(ex.StackTrace)}",
                    "Generic", DiagnosticSeverity.Error, true),
                    Location.None,
                    string.Empty));
            }
        }

        // By not inlining we make sure we can catch assembly loading errors when jitting this method
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void ExecuteInternal(SourceGeneratorContext context)
        {
            context.AddSource("Kontent_Models_Attribute", SourceText.From(attributeText, Encoding.UTF8));

            IEnumerable<string> projectIds = GetProjectIdentifiers(context.Compilation);

            foreach(var projectId in projectIds)
            {
                GenerateModels(context, projectId);
            }
        }


        static IEnumerable<string> GetProjectIdentifiers(Compilation compilation)
        {
            // Get all KontentModels attributes
            IEnumerable<SyntaxNode>? allNodes = compilation.SyntaxTrees.SelectMany(s => s.GetRoot().DescendantNodes());
            IEnumerable<AttributeSyntax> allAttributes = allNodes.Where((d) => d.IsKind(SyntaxKind.Attribute)).OfType<AttributeSyntax>();
            ImmutableArray<AttributeSyntax> attributes = allAttributes.Where(d => d.Name.ToString() == "KontentModels")
                .ToImmutableArray();

            IEnumerable<SemanticModel> models = compilation.SyntaxTrees.Select(st => compilation.GetSemanticModel(st));
            foreach (AttributeSyntax att in attributes)
            {
                string projectId = "";
                int index = 0;

                if (att.ArgumentList is null) throw new Exception("Can't be null here");

                SemanticModel m = compilation.GetSemanticModel(att.SyntaxTree);

                foreach (AttributeArgumentSyntax arg in att.ArgumentList.Arguments)
                {
                    ExpressionSyntax expr = arg.Expression;

                    TypeInfo t = m.GetTypeInfo(expr);
                    Optional<object> v = m.GetConstantValue(expr);
                    if (index == 0)
                    {
                        projectId = v.ToString();
                    }
                    index += 1;
                }
                yield return projectId;
            }
        }

        private static void GenerateModels(SourceGeneratorContext context, string projectId)
        {
            DeliveryOptions deliveryOptions = new DeliveryOptions() { ProjectId = projectId };
            CodeGeneratorOptions codeGeneratorOptions = new CodeGeneratorOptions()
            {
                DeliveryOptions = deliveryOptions,
            };
            IDeliveryClient client = new FakeDeliveryClient();

            CodeGenerator generator = new CodeGenerator(Options.Create(codeGeneratorOptions), client, new RoslynOutputProvider(context));
            generator.RunAsync().GetAwaiter().GetResult();
        }
    }
}
