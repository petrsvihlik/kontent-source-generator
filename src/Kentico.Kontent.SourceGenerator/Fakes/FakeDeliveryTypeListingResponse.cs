using Kentico.Kontent.Delivery.Abstractions;
using System;
using System.Collections.Generic;

namespace Kentico.Kontent.SourceGenerator.Fakes
{
    class FakeDeliveryTypeListingResponse : IDeliveryTypeListingResponse
    {
        public IList<IContentType> Types => new List<IContentType>() { new FakeContentType() };

        public IApiResponse ApiResponse => throw new NotImplementedException();

        public IPagination Pagination => throw new NotImplementedException();
    }
}
