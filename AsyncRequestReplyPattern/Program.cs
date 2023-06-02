using Core;
using Core.BackgroundServices;
using Core.Contracts;
using Core.Infrastructure;
using Core.Infrastructure.Rabbit;
using Core.Mediatr.Notifications;
using MediatR;
using OrderWorker.Services;

namespace OrderWorker;

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
        builder.Services.ServiceBuilder<LongRunningJobNotification<IOrderService>>();
        builder.Services.AddTransient<IOrderService, OrderService>();
        builder.Services.QueueBuilder<LongRunningJobNotification<IOrderService>>();

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
    public static void ServiceBuilder<T>(this IServiceCollection services) 
        where T : class, INotification
    {
        if (services is null)
            throw new ArgumentNullException(nameof(services));

        services.AddHostedService<QueuedHostedService<T>>();
        //services.AddSingleton<IBaseQueue<T>, ChannelQueue<T>>();
        
    }

    public static void QueueBuilder<T>(this IServiceCollection services)
        where T : class, INotification
    {
        services.AddSingleton<IBaseQueue<T>, RabbitQueue<T>>();
        services.AddSingleton<IRabbitProducer, RabbitProducer>();
        services.AddSingleton<IRabbitConsumer<T>, RabbitConsumer<T>>();
    }
}