using CatFactsApp.Configurations;

namespace CatFactsApp.Extensions;

public static class CatFactExtensions
{   
    private const string CatFactApiSectionName = "CatFactApi";
    private const string CatFactFileSectionName = "CatFactFile";
    public static WebApplicationBuilder AddCatFactConfigurations(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<CatFactApiOptions>(builder.Configuration.GetSection(CatFactApiSectionName));
        builder.Services.Configure<CatFactFileOptions>(builder.Configuration.GetSection(CatFactFileSectionName));
        return builder;
    }
}
