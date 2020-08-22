using Kentico.Kontent.Delivery.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace Kentico.Kontent.SourceGenerator.Fakes
{
    public class LightContentType : IContentType
    {
        public IDictionary<string, IContentElement> Elements { get; set; }
        public IContentTypeSystemAttributes System { get; set; }

        public LightContentType(Dictionary<string, LightContentElement> elements, LightContentTypeSystemAttributes system)
        {
            System = system;
            Elements = elements.Select(d => new KeyValuePair<string, IContentElement>(d.Key, d.Value)).ToDictionary(k => k.Key, k => k.Value);

            // Initialize codenames
            foreach (var (Codename, Element) in Elements.Where(r => r.Value is LightContentElement).Select(a => (Codename: a.Key, Element: (LightContentElement)a.Value)))
            {
                Element.Codename = Codename;
            }
        }
    }
}
