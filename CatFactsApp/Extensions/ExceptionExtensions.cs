using CatFactsApp.Exceptions.ExceptionHandlers;

namespace CatFactsApp.Extensions;

public static class ExceptionExtensions
{
    public static WebApplicationBuilder AddExceptionHandlers(this WebApplicationBuilder builder)
    {
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
        return builder;
    }
}
