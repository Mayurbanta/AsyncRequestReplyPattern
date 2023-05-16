
using Core;
using Core.BackgroundServices;
using Core.Infrastructure;
using TaxWorker.Services;

namespace TaxWorker;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        //ref: https://www.kevinlloyd.net/in-memory-queue-with-mediatr/
        builder.Services.ServiceBuilder<TaxService>();
        builder.Services.AddTransient<ITaxService, TaxService>();

        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly,
                                                typeof(CoreBase).Assembly);
        });


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

public static class BuilderExtensions
{
    public static void ServiceBuilder<T>(this IServiceCollection services) where T : class, new()
    {
        if (services is null)
            throw new ArgumentNullException(nameof(services));

        services.AddHostedService<QueuedHostedService<T>>();
        services.AddSingleton<IChannelQueue<T>, ChannelQueue<T>>();

    }

}
