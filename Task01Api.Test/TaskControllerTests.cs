using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Task01Api.Controllers;
using Task01Api.Database.Entities;
using Task01Api.Dtos;

namespace Task01Api.Test;

public class TaskControllerTests(UnitTests factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task ShouldCreateNewTask()
    {
        _taskContext.Database.ExecuteSqlRaw("DELETE FROM \"Tasks\"");
        
        // given
        var task = new PostTaskRequest
        {
            Name = "Test name",
            Description = "Test description"
        };

        var sut = new TaskControllers(_taskContext);

        // when
        var result = sut.CreateTask(task);
        var resultTask = result.Value as Tasks;
        
        // then
        var dbTask = _taskContext.Tasks.FirstOrDefault(t => t.Name == "Test name");
        
        Assert.Equal("Test name", resultTask?.Name);
        Assert.Equal("Test description", resultTask?.Description);
        Assert.Equal("Test name", dbTask?.Name);
        Assert.Equal("Test description", dbTask?.Description);
    }
    
    [Fact]
    public async Task ShouldGetListOfTasks()
    {
        // given
        var task = new Tasks
        {
            Id = "100",
            Name = "Test name",
            Description = "Test description"
        };

        _taskContext.Database.ExecuteSqlRaw("DELETE FROM \"Tasks\"");
        
        _taskContext.Tasks.Add(task);
        await _taskContext.SaveChangesAsync();
        
        var sut = new TaskControllers(_taskContext);

        // when
        var result = sut.GetList();
        var resultAction = result as JsonResult;
        var resultTasks = resultAction?.Value as List<Tasks>;
        
        // then
        Assert.Single(resultTasks);
        Assert.Equal("100", resultTasks[0]?.Id);
        Assert.Equal("Test name", resultTasks[0]?.Name);
        Assert.Equal("Test description", resultTasks[0]?.Description);
    }
    
        
    [Fact]
    public async Task ShouldDeleteTask()
    {
        // given
        var task = new Tasks
        {
            Id = "100",
            Name = "Test name",
            Description = "Test description"
        };

        _taskContext.Database.ExecuteSqlRaw("DELETE FROM \"Tasks\"");
        
        _taskContext.Tasks.Add(task);
        await _taskContext.SaveChangesAsync();
        
        var sut = new TaskControllers(_taskContext);

        // when
        sut.DeleteTask("100");
        
        // then
        var dbTask = _taskContext.Tasks.FirstOrDefault(t => t.Name == "Test name");
        Assert.Null(dbTask);
    }
    
    [Fact]
    public async Task ShouldUpdateTask()
    {
        // given
        var task = new Tasks
        {
            Id = "100",
            Name = "Test name",
            Description = "Test description"
        };

        var putTask = new PutTaskRequest
        {
            Id = "100",
            Name = "Name updated",
            Description = "Test description"
        };
        
        _taskContext.Database.ExecuteSqlRaw("DELETE FROM \"Tasks\"");
        
        _taskContext.Tasks.Add(task);
        await _taskContext.SaveChangesAsync();
        
        var sut = new TaskControllers(_taskContext);

        // when
        var result = sut.UpdateTask(putTask);
        var resultAction = result as JsonResult;
        var resultTasks = resultAction?.Value as List<Tasks>;
        
        // then
        var dbTask = _taskContext.Tasks.Find("100");
        
        Assert.Equal("100", dbTask?.Id);
        Assert.Equal("Name updated", dbTask?.Name);
        Assert.Equal("Test description", dbTask?.Description);
    }
}