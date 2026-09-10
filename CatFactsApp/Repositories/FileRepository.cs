using CatFactsApp.Configurations;
using Microsoft.Extensions.Options;

namespace CatFactsApp.Repositories;

public class FileRepository(IOptions<CatFactFileOptions> options) : IFileRepository
{

    public async Task AppendJsonAsync(string json, CancellationToken cancellationToken)
    {
        await File.AppendAllLinesAsync(options.Value.Path, [json], cancellationToken);
    }
}

public interface IFileRepository
{
    Task AppendJsonAsync(string json, CancellationToken cancellationToken);
}

