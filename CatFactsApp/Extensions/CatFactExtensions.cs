using CatFactsApp.Configurations;

namespace CatFactsApp.Extensions;

public static class CatFactExtensions
{
    public static WebApplicationBuilder AddCatFactConfigurations(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<CatFactApiOptions>(builder.Configuration.GetSection("CatFactApi"));
        return builder;
    }
}
