using EuroBankAPI.Data;
using Serilog;
using ServiceChargesWorker;
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.File("C:\\Eurofins Internship\\Temp\\Demos\\HeartBeat.txt")    
    .CreateLogger();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDbContext<AppDbContext>();
        services.AddScoped<AppDbContext>();
        services.AddHostedService<Worker>(); 
    })
    .UseSerilog()
    .Build();

await host.RunAsync();
