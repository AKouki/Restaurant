using Microsoft.EntityFrameworkCore;
using Restaurant;
using Restaurant.Areas.Admin.Validators;
using Restaurant.Data;
using Restaurant.Extensions;
using Restaurant.Models;
using Restaurant.Services.Email;
using Restaurant.Services.ReCaptcha;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddSiteLocalization();

builder.Services.AddRazorPages()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
                factory.Create(typeof(SharedResource));
    });

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddTransient<IEmailService, SendGridEmailService>();
builder.Services.AddTransient<IFileValidator, FileValidator>();
builder.Services.Configure<SendGridOptions>(builder.Configuration.GetSection("SendGridOptions"));
builder.Services.Configure<SiteSettings>(builder.Configuration.GetSection("SiteSettings"));

builder.Services.AddHttpClient<IReCaptchaService, ReCaptchaV3Service>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ReCaptcha:BaseUrl"]!);
});

var app = builder.Build();

// Seed sample data
app.SeedData();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseRequestLocalization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "Admin",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.Run();

public partial class Program { }
