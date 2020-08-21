using Kentico.Kontent.Delivery.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kentico.Kontent.SourceGenerator.Fakes
{
    class FakeDeliveryClient : IDeliveryClient
    {
        public Task<IDeliveryElementResponse> GetContentElementAsync(string contentTypeCodename, string contentElementCodename)
        {
            throw new NotImplementedException();
        }

        public Task<IDeliveryItemResponse<T>> GetItemAsync<T>(string codename, IEnumerable<IQueryParameter> parameters = null)
        {
            throw new NotImplementedException();
        }

        public Task<IDeliveryItemListingResponse<T>> GetItemsAsync<T>(IEnumerable<IQueryParameter> parameters = null)
        {
            throw new NotImplementedException();
        }

        public IDeliveryItemsFeed<T> GetItemsFeed<T>(IEnumerable<IQueryParameter> parameters = null)
        {
            throw new NotImplementedException();
        }

        public Task<IDeliveryTaxonomyListingResponse> GetTaxonomiesAsync(IEnumerable<IQueryParameter> parameters = null)
        {
            throw new NotImplementedException();
        }

        public Task<IDeliveryTaxonomyResponse> GetTaxonomyAsync(string codename)
        {
            throw new NotImplementedException();
        }

        public Task<IDeliveryTypeResponse> GetTypeAsync(string codename)
        {
            throw new NotImplementedException();
        }

        public Task<IDeliveryTypeListingResponse> GetTypesAsync(IEnumerable<IQueryParameter> parameters = null)
        {
            return Task.FromResult<IDeliveryTypeListingResponse>(new FakeDeliveryTypeListingResponse());
        }
    }
}
