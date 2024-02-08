using GovUk.Frontend.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ServiceAssessmentService.Application;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using ServiceAssessmentService.Application.Database;
using ServiceAssessmentService.Application.UseCases;
using ServiceAssessmentService.WebApp.Models;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using ServiceAssessmentService.WebApp;


var builder = WebApplication.CreateBuilder(args);

var initialScopes = builder.Configuration["DownstreamApi:Scopes"]?.Split(' ') ??
                    builder.Configuration["MicrosoftGraph:Scopes"]?.Split(' ');

// Add services to the container.
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
    .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
    .AddMicrosoftGraph(builder.Configuration.GetSection("MicrosoftGraph"))
    .AddInMemoryTokenCaches();

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy.
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddRazorPages(options =>
    {
        // Convert CamelCase to kebab-case for page routes
        // See also: https://www.gov.uk/guidance/content-design/url-standards-for-gov-uk
        options.Conventions.Add(new PageRouteTransformerConvention(new SlugifyParameterTransformer()));
    })
    .AddMicrosoftIdentityUI();

// // Used for local accounts
// builder.Services
//     .AddDefaultIdentity<ServiceAssessmentServiceWebAppUser>(options => options.SignIn.RequireConfirmedAccount = true)
//     .AddEntityFrameworkStores<DataContext>();

builder.Services.AddGovUkFrontend();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddHealthChecks()
    .AddDbContextCheck<DataContext>();

builder.Services.AddScoped<AssessmentRequestRepository>();
builder.Services.AddScoped<AssessmentTypeRepository>();
builder.Services.AddScoped<PortfolioRepository>();
builder.Services.AddScoped<PhaseRepository>();

builder.Services.AddScoped<GraphUserClient>();

builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.MapHealthChecks("/health").AllowAnonymous();


app.Run();
