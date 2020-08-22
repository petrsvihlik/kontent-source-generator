using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.SourceGenerator.Fakes;
using System.Threading.Tasks;
using Xunit;

namespace Kentico.Kontent.SourceGenerator.Tests
{
    public class FakeDeliveryClientTests
    {
        [Fact]
        public async Task TypesNotEmpty()
        {
            // Arrange
            IDeliveryClient deliveryClient = new FakeDeliveryClient();

            // Act
            var types = await deliveryClient.GetTypesAsync();

            // Assert
            Assert.NotEmpty(types.Types);
        }
    }
}
