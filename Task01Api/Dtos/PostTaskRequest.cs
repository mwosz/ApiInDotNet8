using System.ComponentModel.DataAnnotations;

namespace Task01Api.Dtos;

public class PostTaskRequest
{
    [Required]
    [Length(1, 50)]
    public string Name { get; set; }
    [Required]
    [MinLength(1)]
    public string Description { get; set; }
}