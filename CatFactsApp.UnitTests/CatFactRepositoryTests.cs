using System.Text.Json;
using CatFactsApp.Configurations;
using CatFactsApp.Models;
using CatFactsApp.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Options;

namespace CatFactsApp.UnitTests;

public class CatFactRepositoryTests
{
    [Fact]
    public async Task SaveAsync_Should_WriteCatFactAsJsonLine()
    {
        // Arrange
        var filePath = CreateTempFilePath();
        var repository = CreateRepository(filePath);
        var catFact = new CatFact("Cats are great!", 15);

        try
        {
            // Act
            var result = await repository.SaveAsync(catFact, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();

            var lines = await File.ReadAllLinesAsync(filePath);
            lines.Should().ContainSingle();

            var savedFact = JsonSerializer.Deserialize<CatFact>(lines[0]);
            savedFact.Should().BeEquivalentTo(catFact);
        }
        finally
        {
            DeleteFileIfExists(filePath);
        }
    }

    [Fact]
    public async Task SaveAsync_Should_AppendCatFact_WhenFileAlreadyExists()
    {
        // Arrange
        var filePath = CreateTempFilePath();
        var repository = CreateRepository(filePath);

        try
        {
            // Act
            await repository.SaveAsync(new CatFact("First fact", 10), default);
            var result = await repository.SaveAsync(new CatFact("Second fact", 11), default);

            // Assert
            result.IsSuccess.Should().BeTrue();

            var lines = await File.ReadAllLinesAsync(filePath);
            lines.Should().HaveCount(2);
        }
        finally
        {
            DeleteFileIfExists(filePath);
        }
    }

    [Fact]
    public async Task SaveAsync_Should_ReturnFailure_WhenPathIsInvalid()
    {
        // Arrange
        var invalidPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(invalidPath);
        var repository = CreateRepository(invalidPath);

        try
        {
            // Act
            var result = await repository.SaveAsync(new CatFact("Cats are great!", 15), default);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().NotBeNullOrWhiteSpace();
        }
        finally
        {
            Directory.Delete(invalidPath);
        }
    }

    private static CatFactRepository CreateRepository(string filePath)
    {
        var options = Options.Create(new CatFactFileOptions
        {
            Path = filePath
        });

        return new CatFactRepository(options);
    }

    private static string CreateTempFilePath()
    {
        return Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.txt");
    }

    private static void DeleteFileIfExists(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}
