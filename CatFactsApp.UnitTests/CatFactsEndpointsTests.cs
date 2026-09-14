using CatFactsApp.CustomResults;
using CatFactsApp.Endpoints;
using CatFactsApp.Models;
using CatFactsApp.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;

namespace CatFactsApp.UnitTests;

public class CatFactsEndpointsTests
{
    private readonly ICatFactService _catFactServiceMock;

    public CatFactsEndpointsTests()
    {
        _catFactServiceMock = Substitute.For<ICatFactService>();
    }

    [Fact]
    public async Task GetCatFactAsync_Should_ReturnOk_WhenServiceReturnsSuccess()
    {
        // Arrange
        var catFact = new CatFact("Cats are great!", 15);
        _catFactServiceMock.SaveFactAsync(default)
            .Returns(Result<CatFact>.Success(catFact));

        // Act
        var result = await CatFactsEndpoints.GetCatFactAsync(
            _catFactServiceMock,
            default);

        // Assert
        var okResult = result.Should().BeOfType<Ok<CatFact>>().Subject;
        okResult.Value.Should().BeEquivalentTo(catFact);
    }

    [Fact]
    public async Task GetCatFactAsync_Should_ReturnProblem_WhenServiceReturnsFailure()
    {
        // Arrange
        _catFactServiceMock.SaveFactAsync(default)
            .Returns(Result<CatFact>.Failure("Failed to save cat fact"));

        // Act
        var result = await CatFactsEndpoints.GetCatFactAsync(
            _catFactServiceMock,
            default);

        // Assert
        var problemResult = result.Should().BeOfType<ProblemHttpResult>().Subject;
        problemResult.ProblemDetails.Detail.Should().Be("Failed to save cat fact");
    }
}
