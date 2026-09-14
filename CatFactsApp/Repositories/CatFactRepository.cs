using CatFactsApp.Configurations;
using CatFactsApp.CustomResults;
using CatFactsApp.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CatFactsApp.Repositories;

public class CatFactRepository(IOptions<CatFactFileOptions> options) : ICatFactRepository
{

    public async Task<Result> SaveAsync(CatFact fact, CancellationToken cancellationToken)
    {
        try
        {
            var json = JsonSerializer.Serialize(fact);
            await File.AppendAllLinesAsync(options.Value.Path, [json], cancellationToken);
            return Result.Success();
        }
        catch (IOException ex)
        {
            return Result.Failure(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Result.Failure(ex.Message);
        }
        catch (NotSupportedException ex)
        {
            return Result.Failure(ex.Message);
        }
    }
}

public interface ICatFactRepository
{
    Task<Result> SaveAsync(CatFact fact, CancellationToken cancellationToken);
}

