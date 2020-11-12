using System.Text;
using Kentico.Kontent.ModelGenerator.Core;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Kentico.Kontent.SourceGenerator
{
    public class RoslynOutputProvider : IOutputProvider
    {
        private readonly GeneratorExecutionContext _context;

        public RoslynOutputProvider(GeneratorExecutionContext context)
        {
            _context = context;
        }

        public void Output(string content, string fileName, bool overwriteExisting)
        {
            if (overwriteExisting)
            {
                _context.AddSource(fileName, SourceText.From(content, Encoding.UTF8));
            }
        }
    }
}