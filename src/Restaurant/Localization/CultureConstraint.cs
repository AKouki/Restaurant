using Microsoft.Extensions.Options;

namespace Restaurant.Localization
{
    public class CultureConstraint : IRouteConstraint
    {
        private readonly RequestLocalizationOptions _options;

        public CultureConstraint(IOptions<RequestLocalizationOptions> options)
        {
            _options = options.Value;
        }

        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var supportedCultures = _options.SupportedCultures;
            var requestCulture = values[routeKey]?.ToString()?.ToLowerInvariant();

            if (supportedCultures != null && 
                supportedCultures.Any(c => c.Name.ToLowerInvariant() == requestCulture) && 
                requestCulture?.Length == 2)
                return true;

            return false;
        }
    }
}
