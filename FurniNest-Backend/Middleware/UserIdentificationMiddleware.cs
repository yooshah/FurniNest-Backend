using System.Security.Claims;

namespace FurniNest_Backend.Middleware
{
    public class UserIdentificationMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public UserIdentificationMiddleware(RequestDelegate next, ILogger<UserIdentificationMiddleware> logger) 
        { 
            _next = next;
            _logger = logger;

        }
        public async Task InvokeAsync(HttpContext context)
        {

            if (context.User.Identity?.IsAuthenticated == true)
            {
                var idClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);

                if (idClaim != null)
                {
                    context.Items["UserId"] = idClaim.Value;
                    _logger.LogInformation($"userId-{idClaim.Value}");
                }
                else
                {
                    _logger.LogWarning("'NameIdentifier' not found in JWT Token");
                }

            }
            await _next(context);

        }
    }
}
