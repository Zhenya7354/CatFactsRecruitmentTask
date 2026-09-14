using System.Net;
using CatFactsApp.Configurations;
using CatFactsApp.Models;
using CatFactsApp.Services;
using FluentAssertions;
using Microsoft.Extensions.Options;

namespace CatFactsApp.UnitTests;

public class CatFactClientTests
{
    [Fact]
    public async Task GetCatFactAsync_Should_ReturnSuccess_WhenApiReturnsValidResponse()
    {
        // Arrange
        var handler = new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("""{"fact":"Cats are great!","length":15}""")
        });
        var client = CreateClient(handler);

        // Act
        var result = await client.GetCatFactAsync(default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ErrorMessage.Should().BeNull();
        result.Value.Should().BeEquivalentTo(new CatFact("Cats are great!", 15));
    }

    [Fact]
    public async Task GetCatFactAsync_Should_ReturnFailure_WhenApiReturnsInvalidJson()
    {
        // Arrange
        var handler = new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("invalid json")
        });
        var client = CreateClient(handler);

        // Act
        var result = await client.GetCatFactAsync(default);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
        result.ErrorMessage.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task GetCatFactAsync_Should_ReturnFailure_WhenApiReturnsServerError()
    {
        // Arrange
        var handler = new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.InternalServerError));
        var client = CreateClient(handler);

        // Act
        var result = await client.GetCatFactAsync(default);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
        result.ErrorMessage.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task GetCatFactAsync_Should_CallConfiguredApiUrl()
    {
        // Arrange
        var handler = new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("""{"fact":"Cats are great!","length":15}""")
        });
        var client = CreateClient(handler);

        // Act
        await client.GetCatFactAsync(default);

        // Assert
        handler.RequestUri.Should().Be("https://catfact.ninja/fact");
    }

    private static CatFactClient CreateClient(FakeHttpMessageHandler handler)
    {
        var httpClient = new HttpClient(handler);
        var options = Options.Create(new CatFactApiOptions
        {
            BaseUrl = "https://catfact.ninja",
            Endpoint = "/fact"
        });

        return new CatFactClient(httpClient, options);
    }

    private sealed class FakeHttpMessageHandler(HttpResponseMessage response) : HttpMessageHandler
    {
        public string? RequestUri { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            RequestUri = request.RequestUri?.ToString();
            return Task.FromResult(response);
        }
    }
}
