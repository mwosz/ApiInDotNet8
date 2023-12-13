using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Task01Api.Database;
using Testcontainers.PostgreSql;

namespace Task01Api.Test;

public class UnitTests: WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:14-alpine")
        .WithDatabase("testApp")
        .WithUsername("postgres")
        .WithPassword("password")
        .Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<TaskContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<TaskContext>(options =>
            {
                options
                    .UseNpgsql(_dbContainer.GetConnectionString());
            });
        });

        Environment.SetEnvironmentVariable("POSTGRES_HOST", "localhost");
        Environment.SetEnvironmentVariable("POSTGRES_DB", "testApp");
        Environment.SetEnvironmentVariable("POSTGRES_USERNAME", "postgres");
        Environment.SetEnvironmentVariable("POSTGRES_PASSWORD", "password");
        base.ConfigureWebHost(builder);
    }

    public Task InitializeAsync()
    {
        return _dbContainer.StartAsync();
    }

    public new Task DisposeAsync()
    {
        return _dbContainer.StopAsync();
    }
}