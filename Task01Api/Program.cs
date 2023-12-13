using Microsoft.EntityFrameworkCore;
using Task01Api.Configs;
using Task01Api.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Env variables
var configuration = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();
builder.Services.AddOptions()
    .Configure<Database>(configuration);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<TaskContext>();

builder.Services.AddMvcCore()
    .AddFormatterMappings();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

// Migrate latest database changes during startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<TaskContext>();
    
    // Here is the migration executed
    dbContext.Database.Migrate();
}

app.Run();

public partial class Program { }
