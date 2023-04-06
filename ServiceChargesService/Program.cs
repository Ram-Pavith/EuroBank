using EuroBankAPI.Repository.IRepository;
using EuroBankAPI.Repository;
using Serilog;
using ServiceChargesService;
using EuroBankAPI.Data;
using Microsoft.EntityFrameworkCore;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.File("C:\\Eurofins Internship\\Temp\\Demos\\HeartBeat.txt")
    .CreateLogger();



IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureServices((hostcontext,services) =>
    {
        IConfiguration configuration = hostcontext.Configuration;
        //services.AddDbContext<EuroBankContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

      //  var OptionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
      //  OptionBuilder.UseSqlServer("Server=DESKTOP-LJOJLTJ\\SQLEXPRESS;Database=EuroBank;Trusted_Connection=True;TrustServerCertificate=True;");
        services.AddDbContext<EuroBankContext>(options => options.UseSqlServer("Server=DESKTOP-LJOJLTJ\\SQLEXPRESS;Database=EuroBank;Trusted_Connection=True;TrustServerCertificate=True;"));
       services.AddTransient<EuroBankContext>();
        services.AddHostedService<Worker>();
    })
    .UseSerilog()
    .Build();

await host.RunAsync();
