using Kentico.Kontent.Delivery.Abstractions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kentico.Kontent.SourceGenerator.Fakes
{
    public class LightDeliveryClient : IDeliveryClient
    {
        public LightDeliveryClient(DeliveryOptions options)
        {
            Options = options;
        }

        public DeliveryOptions Options { get; }

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

        public async Task<IDeliveryTypeListingResponse> GetTypesAsync(IEnumerable<IQueryParameter> parameters = null)
        {
            HttpClient http = new();
            var response = await http.GetAsync($"https://deliver.kontent.ai/{Options.ProjectId}/types");
            var body = await response.Content.ReadAsStringAsync();

            var jObject = JObject.Parse(body);

            var types = jObject["types"].ToObject<List<LightContentType>>();

            var typesResponse = new LightDeliveryTypeListingResponse
            {
                Types = types.ToList<IContentType>()                
            };
            return typesResponse;
        }
    }
}
