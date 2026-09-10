using CatFactsApp.Models;

namespace CatFactsApp.Services;

public class CatFactService
    (ICatFactClient apiProvider,
    IFileService fileService): ICatFactService
{
    public async Task<CatFact> SaveFactAsync(CancellationToken cancellationToken)
    {
        var fact = await apiProvider.GetCatFactAsync(cancellationToken);
        var result = await fileService.AppendJsonAsync(fact, cancellationToken);
        if(!result.IsSuccess)
        {
            throw new InvalidOperationException(result.ErrorMessage);
        }
        return fact;
    }
}

interface ICatFactService
{
    Task<CatFact> SaveFactAsync(CancellationToken cancellationToken);
}
