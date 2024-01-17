using GovUk.Frontend.AspNetCore;
using System.Diagnostics.CodeAnalysis;
using Bogus;
using ServiceAssessmentService.WebApp.Services.Book;
using ServiceAssessmentService.WebApp.Services.Lookups;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddGovUkFrontend();



// Generate consistent random data for testing purposes.
Faker.DefaultStrictMode = true;

// Single data store, shared between requests/scopes...
builder.Services.AddScoped<IDummyDataStore, DummyDataStore>(); // Should be scoped, and this allows tests to run, but means a new (freshly seeded) dummy data store is created every request and changes made during running of the app are not "persisted" to the store...
builder.Services.AddScoped<ILookupsReadService, DummyLookupsReadService>();
builder.Services.AddScoped<IBookingRequestReadService, DummyInMemoryBookingRequestReadService>();
builder.Services.AddScoped<IBookingRequestWriteService, DummyInMemoryBookingRequestWriteService>();

builder.Services.AddApplicationInsightsTelemetry();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


// Routes which include a booking request ID:
// - Where an ID is provided, insert it into the middle of the URL (before the action).
// - Booking requests have the action following the (non-optional) ID,
//   ... because it is an action specifically about the request associated with the given ID.
app.MapControllerRoute(
    name: "BookingRequest",
    pattern: "{area=Book}/{controller=BookingRequest}/{id}/{action}");

// Routes which are not request-specific, therefore do not include an ID:
app.MapControllerRoute(
    name: "BookingRequestGeneral",
    pattern: "{area=Book}/{controller=BookingRequest}/{action}");


//// Generic catch-alls which do not match specific routes defined above...
//app.MapControllerRoute(
//    name: "GenericArea",
//    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();


// Partial class exposes this class, allowing test projects to create a test server based on configuration specified in this file.
// See also: https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-7.0#basic-tests-with-the-default-webapplicationfactory
[ExcludeFromCodeCoverage]
public abstract partial class Program { }

