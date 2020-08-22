using System;
using System.Threading.Tasks;
using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.Delivery.Builders.DeliveryClient;
using KenticoKontentModels;
using static Kentico.Kontent.SourceGenerator.Demo.Program;

[assembly: KontentModels(ProjectId)]

namespace Kentico.Kontent.SourceGenerator.Demo
{
    class Program
    {
        public const string ProjectId = "975bf280-fd91-488c-994c-2f04416e5ee3";

        public static async Task Main(string[] args)
        {
            IDeliveryClient client = DeliveryClientBuilder.WithProjectId(ProjectId).WithTypeProvider(new CustomTypeProvider()).Build();
            var articles = await client.GetItemsAsync<Article>();
            foreach (var article in articles.Items)
            {
                Console.WriteLine($"The article '{article.Title}' was posted on {article.PostDate.Value.ToShortDateString()}.");
            }
            Console.ReadLine();
        }
    }
}
