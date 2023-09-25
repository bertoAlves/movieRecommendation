using AVProduct.Configuration;
using AVProduct.Factories;
using AVProduct.Factories.Interfaces;
using AVProduct.Services;
using AVProduct.Services.Interfaces;
using Common.HttpClientWrapper;
using Common.Middleware;

public sealed class Startup
{
    public IConfiguration _configuration { get; }

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var movieDbApiKey = _configuration["TheMovieDB:ServiceApiKey"];

        services.AddDefaultServices();
        services.AddScoped<IMovieFactory, MovieFactory>();
        services.AddScoped<IMovieDetailsFactory, MovieDetailsFactory>();
        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<IMovieDetailsService, MovieDetailsService>();
        services.AddScoped<IKeywordService, KeywordService>();
        services.AddScoped<ITVShowService, TVShowService>();
        services.AddScoped<IDocumentaryService, DocumentaryService>();
        services.Configure<MovieDBApiOptions>(_configuration.GetSection("MovieDbApiOptions"));

        services.AddHttpClient("MovieDbApi", client =>
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {movieDbApiKey}");
        });

        services.AddTransient<IHttpClientWrapper, HttpClientMovieWrapper>();

        services.AddLogging(builder =>
        {
            builder.AddConsole();
        });

        services.AddMemoryCache();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseMiddleware<Middleware>();

        app.UseDefaultAppConfig();
    }

}
