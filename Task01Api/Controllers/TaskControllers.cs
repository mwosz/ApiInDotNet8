using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Task01Api.Database;
using Task01Api.Database.Entities;
using Task01Api.Dtos;

namespace Task01Api.Controllers;

[ApiController]
[Route("/tasks")]
public class TaskControllers(TaskContext taskContext) : ControllerBase
{
    [HttpGet]
    public IActionResult GetList()
    {
        return new JsonResult(taskContext.Tasks.ToList());
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public JsonResult CreateTask([FromBody] PostTaskRequest body)
    {
        var task = new Tasks
        {
            Id = Guid.NewGuid().ToString(),
            Name = body.Name,
            Description = body.Description
        };

        taskContext.Tasks.Add(task);
        taskContext.SaveChanges();
        if (Response != null)
        {
            Response.StatusCode = StatusCodes.Status201Created;
        }
        
        return new JsonResult(task);
    }

    [HttpDelete("{Id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteTask([FromRoute] string id)
    {
        var task = taskContext.Tasks.Find(id);
        if (task == null)
        {
            return NotFound();
        }

        taskContext.Remove(task);
        taskContext.SaveChanges();

        return NoContent();
    }
    
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UpdateTask([FromBody] PutTaskRequest body)
    {
        var task = taskContext.Tasks.Find(body.Id);
        if (task == null)
        {
            return NotFound();
        }

        task.Name = body.Name;
        task.Description = body.Description;
        
        taskContext.Tasks.Update(task);
        taskContext.SaveChanges();
        
        return NoContent();
    }
}