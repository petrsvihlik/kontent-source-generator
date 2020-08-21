using System.Text;
using Kentico.Kontent.ModelGenerator.Core;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Kentico.Kontent.SourceGenerator
{
    public class RoslynOutputProvider : IOutputProvider
    {
        private readonly SourceGeneratorContext _context;

        public RoslynOutputProvider(SourceGeneratorContext context)
        {
            _context = context;
        }

        public void Output(string content, string fileName, bool overwriteExisting)
        {
            _context.AddSource(fileName, SourceText.From(content, Encoding.UTF8));
        }
    }
}