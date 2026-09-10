using CatFactsApp.Models;
using System.Text.Json;

namespace CatFactsApp.Services;

public class FileService : IFileService
{
    private readonly string _path = "CatFacts.txt";

    public async Task AppendJsonAsync<T>(T value, CancellationToken cancellationToken) 
    {
        var json = JsonSerializer.Serialize(value);
        await File.AppendAllLinesAsync(_path, [json], cancellationToken);
    }
}
    public interface IFileService
    {
    public Task AppendJsonAsync<T>(T value, CancellationToken cancellationToken);
    }
