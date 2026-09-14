using CatFactsApp.Repositories;
using CatFactsApp.Services;

namespace CatFactsApp.Extensions;

public static class ServiceExtensions
{
    public static WebApplicationBuilder AddAppServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ICatFactRepository, CatFactRepository>();
        builder.Services.AddScoped<ICatFactService, CatFactService>();
        builder.Services.AddHttpClient<ICatFactClient, CatFactClient>();
        return builder;
    }
}
