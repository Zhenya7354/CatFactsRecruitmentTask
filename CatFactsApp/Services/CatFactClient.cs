using CatFactsApp.Configurations;
using CatFactsApp.Models;
using Microsoft.Extensions.Options;

namespace CatFactsApp.Services;

public class CatFactClient(
    HttpClient httpClient,
    IOptions<CatFactApiOptions> options) : ICatFactClient
{
    private readonly string _apiUrl = options.Value.BaseUrl + options.Value.Endpoint;
    public async Task<CatFact> GetCatFactAsync(CancellationToken cancellationToken)
    {
        var response = await httpClient.GetFromJsonAsync<CatFactApiResponse>(_apiUrl, cancellationToken);
        if (response is null)
        {
            throw new InvalidOperationException("Cat fact API returned an empty response.");
        }
        return new CatFact(response.Fact, response.Length);
    }

}

public interface ICatFactClient
{
    Task<CatFact> GetCatFactAsync(CancellationToken cancellationToken);
}