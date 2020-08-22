using System;
using System.Threading.Tasks;
using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.Delivery.Builders.DeliveryClient;
using KenticoKontentModels;

namespace Kentico.Kontent.SourceGeneratorDemo
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            IDeliveryClient client = DeliveryClientBuilder.WithProjectId("975bf280-fd91-488c-994c-2f04416e5ee3").WithTypeProvider(new CustomTypeProvider()).Build();
            var articles = await client.GetItemsAsync<Article>();
            foreach (var article in articles.Items)
            {
                Console.WriteLine($"Title: {article.Title}");
            }
            Console.ReadLine();
        }
    }
}
