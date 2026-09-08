using CatFactsApp.Configurations;
using CatFactsApp.Models;
using Microsoft.Extensions.Options;

namespace CatFactsApp.Services;

public class CatFactApiProvider(
    HttpClient httpClient,
    IOptions<CatFactApiOptions> options,
    IFileService fileService) : ICatFactApiProvider
{
    private readonly string _apiUrl = options.Value.BaseUrl + options.Value.Endpoint;
    public async Task<CatFactApiResponse> GetCatFactAsync(CancellationToken cancellationToken)
    {
        var response = await httpClient.GetFromJsonAsync<CatFactApiResponse>(_apiUrl, cancellationToken);
        return response ?? throw new InvalidOperationException("Failed to retrieve cat fact");
    }

    public async Task SaveCatFactToFileAsync(CatFactApiResponse fact, CancellationToken cancellationToken)
    {
        await fileService.SaveToFileAsync(fact, cancellationToken);
    }
}

public interface ICatFactApiProvider
{
    Task<CatFactApiResponse> GetCatFactAsync(CancellationToken cancellationToken);
    Task SaveCatFactToFileAsync(CatFactApiResponse fact, CancellationToken cancellationToken);
}