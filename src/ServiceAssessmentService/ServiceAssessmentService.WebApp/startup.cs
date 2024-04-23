using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceAssessmentService.WebApp.Interfaces;
using ServiceAssessmentService.WebApp.Services; // Import the namespace where your service is located

namespace ServiceAssessmentService.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container
            services.AddControllers();

            // Add your service here
            services.AddScoped<ICreateUserService, CreateUserService>(); // Example assuming IMyService is your service interface and MyService is its implementation
            services.AddScoped<IMagicLinkService, MagicLinkService>(); // Example assuming IMagicLinkService is your service interface and MagicLinkService is its implementation
            services.AddScoped<IEmailService, EmailService>(); // Example assuming INotificationService is your service interface and NotificationService is its implementation
            // Other services can be added here
            services.AddAutoMapper(typeof(Startup)); // Add AutoMapper
            // Example: Add a database context
            // services.AddDbContext<YourDbContext>(options =>
            //     options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Example: Add authentication
            // services.AddAuthentication(options =>
            // {
            //     options.DefaultAuthenticateScheme = "YourAuthScheme";
            //     options.DefaultChallengeScheme = "YourAuthScheme";
            // })
            // .AddYourAuthScheme(options => { /* Configure your authentication scheme here */ });

            // Example: Add authorization
            // services.AddAuthorization(options =>
            // {
            //     options.AddPolicy("YourPolicy", policy =>
            //     {
            //         policy.RequireAuthenticatedUser();
            //         policy.RequireRole("Admin");
            //     });
            // });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
