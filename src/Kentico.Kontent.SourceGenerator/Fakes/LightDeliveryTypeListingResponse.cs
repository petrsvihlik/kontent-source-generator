using Kentico.Kontent.Delivery.Abstractions;
using System.Collections.Generic;

namespace Kentico.Kontent.SourceGenerator.Fakes
{
    public class LightDeliveryTypeListingResponse : IDeliveryTypeListingResponse
    {
        public IList<IContentType> Types { get; set; }

        public IApiResponse ApiResponse { get; set; }

        public IPagination Pagination { get; set; }
    }
}
