using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using RestaurantBookingApp.Data;
using RestaurantBookingApp.Service;
using RestaurantTableBookingApp.API;
using RestaurantTableBookingApp.API.RateLimiting;
using Serilog;
using System.Net;

internal class Program
{
    private static void Main(string[] args)
    {
        // Configure serilog with the appsettings.json settings
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .CreateBootstrapLogger();

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            /* All details are available in https://github.com/serilog-contrib/serilog-sinks-applicationinsights */
            builder.Services.AddApplicationInsightsTelemetry();
            builder.Host.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
                        .WriteTo.ApplicationInsights(
                            services.GetRequiredService<TelemetryConfiguration>(),
                                TelemetryConverter.Events));

            // Add services to the container.

            builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>(); // DI Configuration, one instance per http request, that means, if we need the instance in several places like in conroller then services, same instance copy would be shared in that request.
            builder.Services.AddScoped<IRestaurantService, RestaurantService>(); // DI Configuration, one instance per http request, that means, if we need the instance in several places like in conroller then services, same instance copy would be shared in that request.
            builder.Services.AddScoped<IStorageRepository, StorageRepository>();
            builder.Services.AddScoped<IStorageService, StorageService>();
            builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();

            /*
            builder.Services.AddTransient<IRestaurantRepository, RestaurantRepository>(); // DI Configuration, every single time you inject/ask for instance, a new instance is given.
            builder.Services.AddSingleton<IRestaurantRepository, RestaurantRepository>(); // DI Configuration, for the whole app only one instance would be created. It is danger to use as it is not thread safe.
            */

            builder.Services.AddDbContext<RestaurantBookingDBContext>(options =>
            {
                var serviceProvider = builder.Services.BuildServiceProvider();
                var cacheService = serviceProvider.GetRequiredService<IRedisCacheService>();
                string key = "DbConnectionString";
                string? dbConnectionString = cacheService.GetData(key);
                if (string.IsNullOrEmpty(dbConnectionString))
                {
                    dbConnectionString = KeyVaultSecretReader.GetConnectionString(builder.Configuration, "DbKeyVault");
                    cacheService.CacheData(key, dbConnectionString);
                }
                var connectionString = builder.Configuration.GetConnectionString("AzureDBConnectionString"); // In production this will not be null, but in Development it will be null
                connectionString = string.IsNullOrEmpty(connectionString) ? dbConnectionString : connectionString;
                options.UseSqlServer(connectionString)
                .EnableSensitiveDataLogging(); // should not be used in production
            });

            // Rate limiting - is restricting the consumer from accessing the API based on the number of request.
            // 3 reasons to implement this            
            //    1.    Improves security - we can limit, how many times the endpoint can be called. This way we can prevent the bruteforce and Denial-of-service (DoS)
            //    2.    Saves money
            //    3.    Allows implementing paid APIs

            /* 
             *  .net core 7 provides rate limiting out of the box, but will implement Rate limiting considering the framework below .net 7 
             * 
            builder.Services.AddRateLimiter(rateLimiterOptions =>
            {
                rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
                {
                    options.PermitLimit = 1;
                    options.Window = TimeSpan.FromSeconds(5);
                    options.QueueLimit = 0;
                    options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
                });
            });
            */

            Log.Information("Starting the application....");





            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            //Exception handling.
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = exceptionHandlerPathFeature?.Error;

                    Log.Error(exception, "Unhandled exception occurred. {ExceptionDetails}", exception?.ToString());

                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    await context.Response.WriteAsync("An unexpected error occurred. Please try again later");
                });
            });

            app.UseMiddleware<RateLimitingMiddlware>();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();



            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            // app.UseRateLimiter();

            app.MapControllers();

            app.Run();
        }
        catch (Exception ex)
        {

            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}