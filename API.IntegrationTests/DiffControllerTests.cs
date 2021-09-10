using System.Net;
using System.Threading.Tasks;
using API.Contracts;
using FluentAssertions;
using Xunit;

namespace API.IntegrationTests
{
    public class DiffControllerTests:IntegrationTest
    {
        [Fact]
        public async Task GetDiff_WithoutIdBeingPresentInDb_ReturnsNotFound()
        {
            // Arrange

            // Act
            var response = await httpClient.GetAsync(ApiRoutes.Diffs.GetDiff.Replace("{id}","1"));
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}