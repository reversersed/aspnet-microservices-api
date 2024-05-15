using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EventsApi.src.Data.Entities;

public class Category
{
    [Key]
    public long Id { get; set; }
    public string Name { get; set; }
}
