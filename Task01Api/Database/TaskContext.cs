using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Task01Api.Database.Entities;

namespace Task01Api.Database;

public class TaskContext(IOptions<Configs.Database> config) : DbContext
{
    private readonly Configs.Database _config = config.Value;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql($@"Host={_config.POSTGRES_HOST};Username={_config.POSTGRES_USERNAME};" 
                                 + $@"Password={_config.POSTGRES_PASSWORD};Database={_config.POSTGRES_DB}");
        base.OnConfiguring(optionsBuilder);
    }
    
    public DbSet<Tasks> Tasks { get; set; }
}