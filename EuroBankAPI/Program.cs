using EuroBankAPI.Data;
using EuroBankAPI.DTOs;
using EuroBankAPI.Repository;
using EuroBankAPI.Repository.IRepository;
using EuroBankAPI.Service.AuthService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<EuroBankContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();

//AuthService Injection
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmployeeRepository,EmployeeRepository>();
//Serilog Logger Setup
// Serilog DB Logging
//Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext().CreateLogger();
//File Logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/EuroBankLogs.log", rollingInterval: RollingInterval.Month)
    .CreateLogger();
builder.Host.UseSerilog();

//AutoMapper
builder.Services.AddAutoMapper(typeof(Mapper));

//Add Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer"),
                    ValidAudience = builder.Configuration.GetValue<string>("Jwt:Audience"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt")["SecretKey"])),
                    ClockSkew = TimeSpan.Zero // Override the default clock skew of 5 mins
                };
            });


//Cors Policy
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});





// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"Bearer {token} \")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
