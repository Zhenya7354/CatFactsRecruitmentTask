using CatFactsApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace CatFactsApp.Endpoints;

public static class CatFactsEndpoints
{
    public static WebApplication MapCatFactsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/catfacts");
        group.MapGet("/", async ([FromServices] ICatFactService catFactService, CancellationToken cancellationToken) =>
        {
            var catFact = await catFactService.SaveFactAsync(cancellationToken);
            return Results.Ok(catFact);
        });
        return app;
    }
}
