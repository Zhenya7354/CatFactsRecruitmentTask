using CatFactsApp.Services;

namespace CatFactsApp.Extensions;

public static class ServiceExtensions
{
    public static WebApplicationBuilder AddAppServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ICatFactApiProvider, CatFactApiProvider>();
        builder.Services.AddScoped<IFileService, FileService>();
        builder.Services.AddHttpClient<ICatFactApiProvider, CatFactApiProvider>();
        return builder;
    }
}
