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
        private const string attributeName = "GenerateKontentModelsFor";

        private string attributeText = $@"
using System;
namespace KenticoKontentModels
{{
    [System.AttributeUsage(System.AttributeTargets.Assembly, AllowMultiple=true)]
    sealed class {attributeName}Attribute : Attribute
    {{
        public string ProjectId {{ get; set; }}

        public {attributeName}Attribute(string projectId)
        {{
            ProjectId = projectId;
        }}
    }}
}}
";
        /// <inheritdoc/>
        public void Initialize(InitializationContext context)
        {
            // No init required
        }

        /// <inheritdoc/>
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

        /// <remarks>By not inlining we make sure we can catch assembly loading errors when jitting this method.</remarks>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void ExecuteInternal(SourceGeneratorContext context)
        {
            // Add the assembly attribute allowing specifying the project identifier
            context.AddSource("Kontent_Models_Attribute", SourceText.From(attributeText, Encoding.UTF8));

            // Load project identifiers from assembly attributes
            IEnumerable<string> projectIds = GetProjectIdentifiers(context.Compilation);

            // Generate POCO models for all collected projects
            foreach (var projectId in projectIds)
            {
                GenerateModels(context, projectId);
            }
        }

        static IEnumerable<string> GetProjectIdentifiers(Compilation compilation)
        {
            // Get all KontentModels attributes
            var allNodes = compilation.SyntaxTrees.SelectMany(s => s.GetRoot().DescendantNodes());
            var allAttributes = allNodes.Where((d) => d.IsKind(SyntaxKind.Attribute)).OfType<AttributeSyntax>();
            var attributes = allAttributes.Where(d => d.Name.ToString() == attributeName)
                .ToImmutableArray();

            var models = compilation.SyntaxTrees.Select(st => compilation.GetSemanticModel(st));
            foreach (var att in attributes)
            {
                string projectId = "";
                int index = 0;

                if (att.ArgumentList is null) throw new Exception("Can't be null here");

                var m = compilation.GetSemanticModel(att.SyntaxTree);

                foreach (AttributeArgumentSyntax arg in att.ArgumentList.Arguments)
                {
                    var expr = arg.Expression;

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
