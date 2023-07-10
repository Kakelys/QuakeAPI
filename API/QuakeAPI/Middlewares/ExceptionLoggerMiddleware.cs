using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuakeAPI.Middlewares
{
    public class ExceptionLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionLoggerMiddleware> _logger;

        public ExceptionLoggerMiddleware(RequestDelegate next, ILogger<ExceptionLoggerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            } 
            catch(Exception ex)
            {
                _logger.LogError(
                    $"  Exception: "+
                    $"\n\tMethod:{context.Request.Method} "+
                    $"\n\tMessage:{ex.Message}");
                
                throw ex;
            }
        }
    }
}