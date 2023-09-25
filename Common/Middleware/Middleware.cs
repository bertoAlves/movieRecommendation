using Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Common.Middleware
{
    /// <summary>
    /// Middleware
    /// </summary>
    public class Middleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<Middleware> _logger;

        /// <summary>
        /// Middleware
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public Middleware(RequestDelegate next, ILogger<Middleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invoke
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotImplementedException ex)
            {
                _logger.LogError("Not implemented exception occurred: {error}", ex);
                context.Response.StatusCode = 501;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync($"Action:{context.Request.Path} not yet implemented.");
            }
            catch (UnsupportedAlgorithmException ex)
            {
                _logger.LogError("Unsupported Algorithm Error occurred: {error}", ex);
                context.Response.StatusCode = 500;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {error}", ex);
                context.Response.StatusCode = 500;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync($"Action:{context.Request.Path} server encountered an error.");
            }
        }
    }
}
