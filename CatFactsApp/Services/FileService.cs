using CatFactsApp.Models;
using CatFactsApp.Repositories;
using CatFactsApp.Results;
using System.Text.Json;

namespace CatFactsApp.Services;

public class FileService(IFileRepository repository) : IFileService
{
    public async Task<Result> AppendJsonAsync<T>(T value, CancellationToken cancellationToken)
    {
        try
        {
            var json = JsonSerializer.Serialize(value);
            await repository.AppendJsonAsync(json, cancellationToken);
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
public interface IFileService
{
    public Task<Result> AppendJsonAsync<T>(T value, CancellationToken cancellationToken);
}
