using CatFactsApp.CustomResults;
using CatFactsApp.Models;
using CatFactsApp.Repositories;
using CatFactsApp.Services;
using FluentAssertions;
using NSubstitute;

namespace CatFactsApp.UnitTests;

public class CatFactServiceTests
{
    private readonly ICatFactClient _catFactClientMock;
    private readonly ICatFactRepository _catFactRepositoryMock;
    private readonly CatFactService _catFactService;

    public CatFactServiceTests()
    {
        _catFactClientMock = Substitute.For<ICatFactClient>();
        _catFactRepositoryMock = Substitute.For<ICatFactRepository>();
        _catFactService = new CatFactService(_catFactClientMock, _catFactRepositoryMock);
    }
    [Fact]
    public async Task CheckSaveFactAsync_Should_ReturnSuccess()
    {
        //Arrange

        _catFactClientMock.GetCatFactAsync(default)
            .Returns(Result<CatFact>.Success(new CatFact("Cats are great!", 15)));
        _catFactRepositoryMock.SaveAsync(Arg.Any<CatFact>(), default)
            .Returns(Result.Success());

        //Act

        var result = await _catFactService.SaveFactAsync(default);

        //Assert

        Assert.True(result.IsSuccess);
        Assert.Null(result.ErrorMessage);
        Assert.NotNull(result.Value);
        result.Value.Length.Should().Be(15);
        result.Value.Fact.Should().Be("Cats are great!");
    }
    [Fact]
    public async Task CheckSaveFactAsync_Should_ReturnFailure_WhenGetCatFactAsync_Fails()
    {
        //Arrange

        _catFactClientMock.GetCatFactAsync(default)
            .Returns(Result<CatFact>.Failure("Failed to get cat fact"));
        _catFactRepositoryMock.SaveAsync(Arg.Any<CatFact>(), default)
            .Returns(Result.Success());

        //Act

        var result = await _catFactService.SaveFactAsync(default);

        //Assert

        Assert.False(result.IsSuccess);
        Assert.NotNull(result.ErrorMessage);
        Assert.Null(result.Value);
        result.ErrorMessage.Should().Be("Failed to get cat fact");
    }

    [Fact]
    public async Task CheckSaveFactAsync_Should_ReturnFailure_WhenGetCatFactAsync_ReturnEmptyValue()
    {
        //Arrange

        _catFactClientMock.GetCatFactAsync(default)
            .Returns(Result<CatFact>.Success(new CatFact("", 0)));
        _catFactRepositoryMock.SaveAsync(Arg.Any<CatFact>(), default)
            .Returns(Result.Success());

        //Act

        var result = await _catFactService.SaveFactAsync(default);

        //Assert

        Assert.False(result.IsSuccess);
        Assert.NotNull(result.ErrorMessage);
        Assert.Null(result.Value);
        result.ErrorMessage.Should().Be("Received empty cat fact from API.");
    }
    [Fact]
    public async Task CheckSaveFactAsync_Should_ReturnFailure_WhenSaveAsync_Fails()
    {
        //Arrange

        _catFactClientMock.GetCatFactAsync(default)
            .Returns(Result<CatFact>.Success(new CatFact("Cats are great!", 15)));
        _catFactRepositoryMock.SaveAsync(Arg.Any<CatFact>(), default)
            .Returns(Result.Failure("Failed to save cat fact"));

        //Act

        var result = await _catFactService.SaveFactAsync(default);

        //Assert

        Assert.False(result.IsSuccess);
        Assert.NotNull(result.ErrorMessage);
        Assert.Null(result.Value);
        result.ErrorMessage.Should().Be("Failed to save cat fact");
        
    }


}
