using System.Configuration;
using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Exceptions.ExceptionMiddleware;
using WebApplication1.Repositories;
using WebApplication1.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var serilogLogger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("middleware-log-.txt",rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Services.AddSingleton<Serilog.ILogger>(serilogLogger);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("WebApiDatabase")));
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddApiVersioning(option =>
{
    option.AssumeDefaultVersionWhenUnspecified = true; 
    option.DefaultApiVersion = new ApiVersion(1, 0);
    option.ReportApiVersions = true; 
    option.ApiVersionReader = ApiVersionReader.Combine(
        new MediaTypeApiVersionReader("ver")); 
}).AddApiExplorer(options => {
    options.GroupNameFormat = "'v'VVV"; 
    options.SubstituteApiVersionInUrl = true; 
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();