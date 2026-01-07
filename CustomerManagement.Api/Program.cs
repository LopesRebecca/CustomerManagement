using CustomerManagement.Api.Extensions;
using CustomerManagement.Domain.Interface.Repositories;
using CustomerManagement.Infrastructure.Persistence;
using CustomerManagement.Infrastructure.Repositories;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
    {
        Title = "Customer Management API",
        Version = "v1",
        Description = "API para gerenciamento de clientes"
    });
});

// NHibernate
builder.Services.AddSingleton(factory =>
    NHibernateSessionFactory.CreateSessionFactory(
        builder.Configuration.GetConnectionString("Default")
        ?? throw new InvalidOperationException("Connection string 'Default' not found.")
    )
);

// Repositories
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

// Application - Mediator e Handlers
builder.Services.AddApplication();


var app = builder.Build();

// Global Exception Handler
app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
        if (contextFeature != null)
        {
            var response = new { error = "An internal error occurred. Please try again later." };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    });
});

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer Management API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
