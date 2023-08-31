using Microsoft.EntityFrameworkCore;
using RestaurantBookingApp.Data;
using RestaurantBookingApp.Service;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddDbContext<RestaurantBookingDBContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("AzureDBConnectionString") ?? "")
            .EnableSensitiveDataLogging(); // should not be used in production
        });

        builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>(); // DI Configuration, one instance per http request, that means, if we need the instance in several places like in conroller then services, same instance copy would be shared in that request.
        builder.Services.AddScoped<IRestaurantService, RestaurantService>(); // DI Configuration, one instance per http request, that means, if we need the instance in several places like in conroller then services, same instance copy would be shared in that request.

        /*
        builder.Services.AddTransient<IRestaurantRepository, RestaurantRepository>(); // DI Configuration, every single time you inject/ask for instance, a new instance is given.
        builder.Services.AddSingleton<IRestaurantRepository, RestaurantRepository>(); // DI Configuration, for the whole app only one instance would be created. It is danger to use as it is not thread safe.
        */

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}