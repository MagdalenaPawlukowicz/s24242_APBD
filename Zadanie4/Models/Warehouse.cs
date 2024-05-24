using System.ComponentModel.DataAnnotations;

namespace Zadanie4.Models;

public class Warehouse
{
    [Required] 
    [Key]
    public int IdWarehouse { get; init; }
    
    [Required]
    [MaxLength(200)]
    public string Name { get; set; }
    
    [Required] 
    [MaxLength(200)]
    public string Address { get; set; }
}