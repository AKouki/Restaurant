using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace Restaurant.IntegrationTests
{
    public class BasicTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public BasicTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/About")]
        [InlineData("/Menu")]
        [InlineData("/Gallery")]
        [InlineData("/Contact")]
        [InlineData("/Reservation")]
        [InlineData("/FAQs")]
        [InlineData("/Terms")]
        [InlineData("/Privacy")]
        [InlineData("/Cookies")]
        public async Task Get_ReturnsSuccessStatusCodeAndContentType(string url)
        {
            // Arange
            var client = _factory.CreateClient();
            var expectedContentType = "text/html; charset=utf-8";

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(expectedContentType, response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("/en")]
        [InlineData("/en/About")]
        [InlineData("/en/Menu")]
        [InlineData("/en/Gallery")]
        [InlineData("/en/Contact")]
        [InlineData("/en/Reservation")]
        [InlineData("/en/FAQs")]
        [InlineData("/en/Terms")]
        [InlineData("/en/Privacy")]
        [InlineData("/en/Cookies")]
        public async Task GetEnglish_ReturnsSuccessStatusCodeAndContentType(string url)
        {
            // Arange
            var client = _factory.CreateClient();
            var expectedContentType = "text/html; charset=utf-8";

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(expectedContentType, response.Content.Headers.ContentType?.ToString());
        }

        [Theory]
        [InlineData("/Admin")]
        [InlineData("/Admin/Reservations")]
        [InlineData("/Admin/Menus")]
        [InlineData("/Admin/Recommendations")]
        [InlineData("/Admin/Reviews")]
        public async Task GetSecurePage_UnauthorizedUser_ReturnsRedirectToLogin(string url)
        {
            // Arange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions() { AllowAutoRedirect = false });
            var expectedLoginUri = "http://localhost/Identity/Account/Login";

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.StartsWith(expectedLoginUri, response.Headers.Location?.OriginalString);
        }
    }
}