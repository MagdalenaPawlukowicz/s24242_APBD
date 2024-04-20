using System.ComponentModel.DataAnnotations;

namespace Zadanie3_s24242.Models;

public class Animal
{
    [Required]
    public int IdAnimal { get; init; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; }
    
    [MaxLength(200)]
    public string Description { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Category { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Area { get; set; }
}