using Microsoft.AspNetCore.Rewrite;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;


namespace APIGateway;

public static class WebApiDefaultConfig
{
    public static IServiceCollection AddDefaultServices(this IServiceCollection services)
    {
        services.AddMvc();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Movie Recommendation API Gateway", Version = "v1" });
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "apigateway.xml"));
        });

        services.AddHttpContextAccessor();

        return services;
    }

    public static IApplicationBuilder UseDefaultAppConfig(this IApplicationBuilder app)
    {
        app.UseSwagger();

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movie Recommendation API Gateway v1");
            c.DocExpansion(DocExpansion.List);
        });

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseRewriter(new RewriteOptions().AddRedirect("^$", "swagger"));

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        return app;
    }

}
