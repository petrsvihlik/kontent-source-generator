using Kentico.Kontent.Delivery.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace Kentico.Kontent.SourceGenerator.Fakes
{
    public class FakeContentType : IContentType
    {
        public IDictionary<string, IContentElement> Elements { get; set; }
        public IContentTypeSystemAttributes System { get; set; }

        public FakeContentType(Dictionary<string, FakeContentElement> elements, FakeContentTypeSystemAttributes system)
        {
            System = system;
            Elements = elements.Select(d => new KeyValuePair<string, IContentElement>(d.Key, d.Value)).ToDictionary(k => k.Key, k => k.Value);

            // Initialize codenames
            foreach (var element in Elements.Where(r => r.Value is FakeContentElement).Select(a => (Codename: a.Key, Element: (FakeContentElement)a.Value)))
            {
                element.Element.Codename = element.Codename;
            }
        }
    }
}
