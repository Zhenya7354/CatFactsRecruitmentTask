using CatFactsApp.Models;
using CatFactsApp.Repositories;

namespace CatFactsApp.Services;

public class CatFactService
    (ICatFactClient apiProvider,
    ICatFactRepository repository): ICatFactService
{
    public async Task<CatFact> SaveFactAsync(CancellationToken cancellationToken)
    {
        var factResult = await apiProvider.GetCatFactAsync(cancellationToken);

        if(!factResult.IsSuccess)
        {
            throw new InvalidOperationException(factResult.ErrorMessage);
        }

        var saveResult = await repository.SaveAsync(factResult.Value, cancellationToken);

        if(!saveResult.IsSuccess)
        {
            throw new InvalidOperationException(saveResult.ErrorMessage);
        }
        return factResult.Value;
    }
}

interface ICatFactService
{
    Task<CatFact> SaveFactAsync(CancellationToken cancellationToken);
}
