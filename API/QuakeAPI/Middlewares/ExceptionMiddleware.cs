using System.Runtime.CompilerServices;
using QuakeAPI.Exceptions;

namespace QuakeAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            } 
            catch(ArgumentNullException ex)
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync($"{ex.Message} is empty");
            }
            catch (BadRequestException ex)
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(ex.Message);
            }
            catch(NotFoundException ex)
            {
                context.Response.StatusCode = 404;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(ex.Message);
            }
            catch
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("Internal server error.");
            }
        }
    }
}