using Kentico.Kontent.Delivery.Abstractions;
using System;
using System.Collections.Generic;

namespace Kentico.Kontent.SourceGenerator.Fakes
{
    class FakeContentType : IContentType
    {
        public IDictionary<string, IContentElement> Elements => new Dictionary<string, IContentElement>()
        {
            { "body_copy", new FakeContentElement("body_copy", "Body Copy", "rich_text") },
            { "post_date", new FakeContentElement("post_date", "Post date", "date_time") },
            { "price", new FakeContentElement("price", "Price", "number") },
            { "metadata__twitter_image", new FakeContentElement("metadata__twitter_image", "Twitter image", "asset") }
        };

        public IContentTypeSystemAttributes System => new FakeContentTypeSystemAttributes("article", Guid.NewGuid().ToString(), DateTime.Now, "Article");
    }
}
