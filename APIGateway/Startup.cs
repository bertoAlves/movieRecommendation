using APIGateway.Configuration;
using APIGateway.Factories;
using APIGateway.Factories.Interfaces;
using APIGateway.Services;
using APIGateway.Services.Interfaces;
using CinemaMS.Services;
using Common.HttpClientWrapper;
using Common.Middleware;

namespace APIGateway;

public sealed class Startup
{
    public IConfiguration _configuration { get; }

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDefaultServices();
        services.AddScoped<IRecommendationService, RecommendationService>();
        services.AddScoped<IAVProductMSService, AVProductMSService>();
        services.AddScoped<ICinemaMSService, CinemaMSService>();
        services.AddScoped<IGenreFactory, GenreFactory>();
        services.AddHttpClient();
        services.AddTransient<IHttpClientWrapper, HttpClientWrapper>();
        services.Configure<MicroservicesOptions>(_configuration.GetSection("MicroservicesOptions"));
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseMiddleware<Middleware>();

        app.UseDefaultAppConfig();
    }

}
