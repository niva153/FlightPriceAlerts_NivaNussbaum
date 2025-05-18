using System.Security.Claims;

namespace FlightPriceAlert.API.Middlewares
{
    public class FakeUserMiddleware
    {
        private readonly RequestDelegate _next;

        public FakeUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.User.Identity?.IsAuthenticated ?? true)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, "11111111-1111-1111-1111-111111111111"),
                    new Claim(ClaimTypes.Email, "fakeuser@example.com"),
                    new Claim(ClaimTypes.Name, "Fake User")
                };

                var identity = new ClaimsIdentity(claims, "FakeAuthentication");
                var principal = new ClaimsPrincipal(identity);
                context.User = principal;
            }

            await _next(context);
        }
    }
    public static class FakeUserMiddlewareExtensions
    {
        public static IApplicationBuilder UseFakeUser(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FakeUserMiddleware>();
        }
    }
}
