using CatFactsApp.Configurations;
using CatFactsApp.CustomResults;
using CatFactsApp.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CatFactsApp.Services;

public class CatFactClient(
    HttpClient httpClient,
    IOptions<CatFactApiOptions> options) : ICatFactClient
{
    private readonly string _apiUrl = options.Value.BaseUrl + options.Value.Endpoint;
    public async Task<Result<CatFact>> GetCatFactAsync(CancellationToken cancellationToken)
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<CatFactApiResponse>(_apiUrl, cancellationToken);

            if (response is null)
            {
                return Result<CatFact>.Failure("Cat fact API returned an empty response.");
            }

            return Result<CatFact>.Success(new CatFact(response.Fact, response.Length));
        }
        catch (HttpRequestException ex)
        {
            return Result<CatFact>.Failure(ex.Message);
        }
        catch (JsonException ex)
        {
            return Result<CatFact>.Failure(ex.Message);
        }
        catch (NotSupportedException ex)
        {
            return Result<CatFact>.Failure(ex.Message);
        }
    }

}

public interface ICatFactClient
{
    Task<Result<CatFact>> GetCatFactAsync(CancellationToken cancellationToken);
}