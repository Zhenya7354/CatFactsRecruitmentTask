using CatFactsApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace CatFactsApp.Endpoints;

public static class CatFactsEndpoints
{
    public static WebApplication MapCatFactsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/catfacts");
        group.MapGet("/", GetCatFactAsync);
        return app;
    }

    public static async Task<IResult> GetCatFactAsync(
        [FromServices] ICatFactService catFactService,
        CancellationToken cancellationToken)
    {
        var result = await catFactService.SaveFactAsync(cancellationToken);
        if (!result.IsSuccess)
        {
            return Results.Problem(result.ErrorMessage);
        }
        return Results.Ok(result.Value);
    }
}

