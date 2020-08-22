using System;
using System.Runtime.CompilerServices;
using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.ModelGenerator.Core;
using Kentico.Kontent.ModelGenerator.Core.Configuration;
using Kentico.Kontent.SourceGenerator.Fakes;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Options;

namespace Kentico.Kontent.SourceGenerator
{
    [Generator]
    public class KontentSourceGenerator : ISourceGenerator
    {
        public void Initialize(InitializationContext context)
        {

        }

        public void Execute(SourceGeneratorContext context)
        {
            try
            {
                ExecuteInternal(context);
            }
            catch (Exception ex)
            {
                context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor("KSG001",
                    "SourceGeneratorError",
                    "MSG: " + ex.Message.Replace("\n", "").Replace("\r", "") + "Stack: " + ex.StackTrace.Replace("\n", "").Replace("\r", "") + "Src: " + ex.Source,
                    "Generic", DiagnosticSeverity.Error, true),
                    Location.None,
                    ex.StackTrace.ToString()));
            }
        }

        // By not inlining we make sure we can catch assembly loading errors when jitting this method
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void ExecuteInternal(SourceGeneratorContext context)
        {
            DeliveryOptions deliveryOptions = new DeliveryOptions() { ProjectId = "975bf280-fd91-488c-994c-2f04416e5ee3" };
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
