using Kentico.Kontent.Delivery.Abstractions;
using System.Collections.Generic;

namespace Kentico.Kontent.SourceGenerator.Fakes
{
    public class FakeDeliveryTypeListingResponse : IDeliveryTypeListingResponse
    {
        public IList<IContentType> Types { get; set; }

        public IApiResponse ApiResponse { get; set; }

        public IPagination Pagination { get; set; }
    }
}
