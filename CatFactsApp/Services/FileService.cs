using CatFactsApp.Models;
using System.Text.Json;

namespace CatFactsApp.Services;

public class FileService : IFileService
{
    private readonly string _path = "CatFacts.txt";

    public async Task SaveToFileAsync(CatFactApiResponse fact, CancellationToken cancellationToken)
    {
        if (fact is null)
        {
            return;
        }
        string factString = "Fact: " + fact.Fact + " | Length: " + fact.Length;
        await File.AppendAllLinesAsync(_path, [factString], cancellationToken);
    }
}
    public interface IFileService
    {
        public Task SaveToFileAsync(CatFactApiResponse fact, CancellationToken cancellationToken);
    }
