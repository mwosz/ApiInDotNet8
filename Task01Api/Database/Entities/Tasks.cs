using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Task01Api.Database.Entities;

[PrimaryKey(nameof(Id))]
public class Tasks
{
    public string Id { get; set; }
    [StringLength(50)]
    public string Name { get; set; }
    public string Description { get; set; }
}