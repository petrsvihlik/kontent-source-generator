﻿using Kentico.Kontent.Delivery.Abstractions;
using System;

namespace Kentico.Kontent.SourceGenerator.Fakes
{
    public class LightContentTypeSystemAttributes : IContentTypeSystemAttributes
    {
        public string Codename { get; set; }

        public string Id { get; set; }

        public DateTime LastModified { get; set; }

        public string Name { get; set; }

        public LightContentTypeSystemAttributes(string codename, string id, DateTime lastModified, string name)
        {
            Codename = codename;
            Id = id;
            LastModified = lastModified;
            Name = name;
        }
    }
}
