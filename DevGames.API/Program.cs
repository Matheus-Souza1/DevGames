using DevGames.API.Persistence;
using DevGames.API.Persistence.Repositories;
using DevGames.API.Profiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    var settings = config.Build();
    Serilog.Log.Logger = new LoggerConfiguration()
     .Enrich.FromLogContext()
     .WriteTo.MSSqlServer(settings.GetConnectionString("DevGamesCs"),
     sinkOptions: new MSSqlServerSinkOptions()
     {
         AutoCreateSqlTable = true,
         TableName = "Logs"
     }).CreateLogger();
}).UseSerilog();

// Add services to the container.

var connection = builder.Configuration.GetConnectionString("DevGamesCs");

builder.Services.AddDbContext<DevGamesContext>(opt => opt.UseSqlServer(connection));
builder.Services.AddScoped<IBoardRepository, BoardRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddAutoMapper(typeof(BoardProfile));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DevGames API",
        Version = "v1",
        Contact = new OpenApiContact
        {
            Name = "Matheus Souza",
            Email = "matheussouzaslv2@gmail.com"
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory,xmlFile);
    x.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
