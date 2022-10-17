#nullable disable
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;
using Restaurant.Localization;
using System.Globalization;

namespace Restaurant.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSiteLocalization(this IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Localization/Resources");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>()
                {
                    new CultureInfo("el"),
                    new CultureInfo("en"),
                    new CultureInfo("en-US") // for Admin Area
                };

                options.DefaultRequestCulture = new RequestCulture("el");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.AddInitialRequestCultureProvider(new CustomRequestCultureProvider(async context =>
                {
                    var culture = supportedCultures[0].Name;
                    var segments = context.Request.Path.Value.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                    if (segments.Length >= 1)
                    {
                        if (segments[0] == "Admin")
                            culture = "en-US";
                        else if (segments[0].Length == 2)
                            culture = segments[0];
                    }

                    return await Task.FromResult(new ProviderCultureResult(new StringSegment(culture)));
                }));
            });

            services.Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AddFolderRouteModelConvention("/", model =>
                {
                    // By default, a page named /folder/index.cshtml will result in two routes (/folder and /folder/index)
                    // We don't want the /folder/index routes
                    model.Selectors
                    .Where(s => s.AttributeRouteModel.Template.EndsWith("Index") || s.AttributeRouteModel.Template == "Index")
                    .ToList()
                    .ForEach(s => model.Selectors.Remove(s));

                    model.Selectors
                    .ToList()
                    .ForEach(s =>
                    {
                        model.Selectors.Add(new SelectorModel()
                        {
                            AttributeRouteModel = new AttributeRouteModel()
                            {
                                Order = -1,
                                Template = AttributeRouteModel.CombineTemplates("{culture:culture-constraint}", s.AttributeRouteModel.Template)
                            }
                        });
                    });
                });
            });

            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("culture-constraint", typeof(CultureConstraint));
            });

            return services;
        }
    }
}
