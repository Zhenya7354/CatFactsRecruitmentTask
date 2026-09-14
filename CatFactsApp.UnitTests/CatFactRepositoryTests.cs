using System.Runtime.CompilerServices;
using System.Text.Json;
using CatFactsApp.Configurations;
using CatFactsApp.Models;
using CatFactsApp.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Options;

namespace CatFactsApp.UnitTests;

public class CatFactRepositoryTests : IDisposable
{
    private static readonly List<string> _tempFiles = [];
    [Fact]
    public async Task SaveAsync_Should_WriteCatFactAsJsonLine()
    {
        // Arrange
        var filePath = CreateTempFilePath();
        var repository = CreateRepository(filePath);
        var catFact = new CatFact("Cats are great!", 15);

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

    [Fact]
    public async Task SaveAsync_Should_AppendCatFact_WhenFileAlreadyExists()
    {
        // Arrange
        var filePath = CreateTempFilePath();
        var repository = CreateRepository(filePath);

            // Act
            await repository.SaveAsync(new CatFact("First fact", 10), default);
            var result = await repository.SaveAsync(new CatFact("Second fact", 11), default);
            var lines = await File.ReadAllLinesAsync(filePath);

            // Assert
            result.IsSuccess.Should().BeTrue();
            lines.Should().HaveCount(2);
        
       
    }

    [Fact]
    public async Task SaveAsync_Should_ReturnFailure_WhenPathIsInvalid()
    {
        // Arrange
        var invalidPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(invalidPath);
        var repository = CreateRepository(invalidPath);

            // Act
            var result = await repository.SaveAsync(new CatFact("Cats are great!", 15), default);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().NotBeNullOrWhiteSpace();
        
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
        var filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.txt");
        _tempFiles.Add(filePath);
        return filePath;
    }
    public void Dispose()
    {
        DeleteFileIfExists();
    }

    private static void DeleteFileIfExists()
    {
        foreach (var filePath in _tempFiles)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

}
