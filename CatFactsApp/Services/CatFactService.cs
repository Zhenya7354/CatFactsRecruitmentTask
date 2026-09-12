using CatFactsApp.CustomResults;
using CatFactsApp.Models;
using CatFactsApp.Repositories;

namespace CatFactsApp.Services;

public class CatFactService
    (ICatFactClient apiProvider,
    ICatFactRepository repository): ICatFactService
{
    public async Task<Result<CatFact>> SaveFactAsync(CancellationToken cancellationToken)
    {
        var factResult = await apiProvider.GetCatFactAsync(cancellationToken);

        if(!factResult.IsSuccess)
        {
            return Result<CatFact>.Failure(factResult.ErrorMessage);
        }

        if(string.IsNullOrEmpty(factResult.Value.Fact) || factResult.Value.Length == 0)
        {
            return Result<CatFact>.Failure("Received empty cat fact from API.");
        }

        var saveResult = await repository.SaveAsync(factResult.Value, cancellationToken);

        if(!saveResult.IsSuccess)
        {
            return Result<CatFact>.Failure(saveResult.ErrorMessage);
        }
        return Result<CatFact>.Success(factResult.Value);
    }
}

interface ICatFactService
{
    Task<Result<CatFact>> SaveFactAsync(CancellationToken cancellationToken);
}
