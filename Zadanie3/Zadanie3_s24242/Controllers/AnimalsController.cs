using Microsoft.AspNetCore.Mvc;
using Zadanie3_s24242.Models;
using Zadanie3_s24242.Repositories;

namespace Zadanie3_s24242.Controllers;

[Route("api/animals")]
[ApiController]
public class AnimalsController : ControllerBase
{
    private IAnimalsRepository _animalsRepository;
    
    public AnimalsController(IAnimalsRepository animalsRepository)
    {
        _animalsRepository = animalsRepository;
    }
    
    
    [HttpGet]
    public ActionResult<IEnumerable<Animal>> GetAnimals(string orderBy = "name")
    {
        var animals = _animalsRepository.GetRepoAnimals().ToList();

        switch (orderBy.ToLower())
        {
            case "description":
                animals.Sort((a1, a2) => string.Compare(a1.Description, a2.Description, StringComparison.OrdinalIgnoreCase));
                break;
            case "category":
                animals.Sort((a1, a2) => string.Compare(a1.Category, a2.Category, StringComparison.OrdinalIgnoreCase));
                break;
            case "area":
                animals.Sort((a1, a2) => string.Compare(a1.Area, a2.Area, StringComparison.OrdinalIgnoreCase));
                break;
            default:
                animals.Sort((a1, a2) => string.Compare(a1.Name, a2.Name, StringComparison.OrdinalIgnoreCase));
                break;
        }

        return animals;
    }

}