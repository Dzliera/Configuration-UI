using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ConfigurationUi.Middlewares
{
    public class ConfigurationUiMiddleware : IMiddleware
    {
        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            throw new System.NotImplementedException();
        }
        
    }
}