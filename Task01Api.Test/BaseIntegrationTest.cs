using Microsoft.Extensions.DependencyInjection;
using Task01Api.Database;

namespace Task01Api.Test;

public abstract class BaseIntegrationTest : IClassFixture<UnitTests>
{
    private readonly IServiceScope _scope;
    protected readonly TaskContext _taskContext;

    protected BaseIntegrationTest(UnitTests factory)
    {
        _scope = factory.Services.CreateScope();
        _taskContext =  _scope.ServiceProvider.GetRequiredService<TaskContext>();
    }
}