using CatFactsApp.Endpoints;
using CatFactsApp.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.AddAppServices();
builder.AddCatFactConfigurations();
builder.AddExceptionHandlers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();
app.MapCatFactsEndpoints();

app.Run();

