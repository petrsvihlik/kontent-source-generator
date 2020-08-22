using Kentico.Kontent.Delivery.Abstractions;

namespace Kentico.Kontent.SourceGenerator.Fakes
{
    public class LightContentElement : IContentElement
    {
        public string Codename { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public LightContentElement(string codename, string name, string type)
        {
            Codename = codename;
            Name = name;
            Type = type;
        }
    }
}
