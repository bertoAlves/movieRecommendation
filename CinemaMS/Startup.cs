using CinemaMS;
using CinemaMS.DAL.Interfaces;
using CinemaMS.DAL;
using CinemaMS.Factories.Interfaces;
using CinemaMS.Factories;
using CinemaMS.Services.Interfaces;
using CinemaMS.Services;
using Common.Middleware;
using CinemaMS.Configuration;

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

        //Services
        services.AddScoped<IGenreService, GenreService>();

        //Factories
        services.AddScoped<IBuildGenreDTO, BuildGenreDTO>();

        //Option
        services.Configure<SuccessfulGenresOptions>(_configuration.GetSection("SuccessfulGenresOptions"));

        //DAL
        services.AddScoped<ISessionDAL, SessionDAL>();
        services.AddScoped<IGenreDAL, GenreDAL>();

        //DBContext
        services.AddDbContext<CinemaContext>();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseMiddleware<Middleware>();

        app.UseDefaultAppConfig();
    }

}
