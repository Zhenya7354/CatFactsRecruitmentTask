using CatFactsApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace CatFactsApp.Endpoints;

public static class CatFactsEndpoints
{
    public static WebApplication MapCatFactsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/catfacts");
        group.MapGet("/", async ([FromServices] ICatFactApiProvider catFactApiProvider, CancellationToken cancellationToken) =>
        {
            var catFact = await catFactApiProvider.GetCatFactAsync(cancellationToken);
            await catFactApiProvider.SaveCatFactToFileAsync(catFact, cancellationToken);
            return Results.Ok(catFact);
        });
        return app;
    }
}
