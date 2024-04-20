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
    
    [HttpPost]
    public ActionResult AddAnimal([FromBody] Animal newAnimal)
    {
        try
        {
            _animalsRepository.AddAnimal(newAnimal);
            return Ok("Zwierzę dodane pomyślnie.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Wystąpił błąd podczas dodawania zwierzęcia: {ex.Message}");
        }
    }
    
    [HttpPut("{idAnimal}")]
    public ActionResult UpdateAnimal(int idAnimal, Animal updatedAnimal) {
        try {
            var animals = _animalsRepository.GetRepoAnimals();
            var existingAnimal = animals.FirstOrDefault(a => a.IdAnimal == idAnimal);

            if (existingAnimal == null)
            {
                return NotFound("Zwierzę o podanym identyfikatorze nie zostało znalezione.");
            }

            existingAnimal.Name = updatedAnimal.Name;
            existingAnimal.Description = updatedAnimal.Description;
            existingAnimal.Category = updatedAnimal.Category;
            existingAnimal.Area = updatedAnimal.Area;

            _animalsRepository.UpdateAnimal(existingAnimal);

            return Ok("Zwierzę zaktualizowane pomyślnie.");
        }catch (Exception ex)
        {
            return StatusCode(500, $"Wystąpił błąd podczas aktualizacji zwierzęcia: {ex.Message}");
        }
    }
    
    [HttpDelete("{idAnimal}")]
    public ActionResult DeleteAnimal(int idAnimal)
    {
        try{
            var animals = _animalsRepository.GetRepoAnimals();
            var existingAnimal = animals.FirstOrDefault(a => a.IdAnimal == idAnimal);

            if (existingAnimal == null) { return NotFound("Zwierzę o podanym identyfikatorze nie zostało znalezione."); }

            _animalsRepository.DeleteAnimal(idAnimal);
            return Ok("Zwierzę usunięte pomyślnie.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Wystąpił błąd podczas usuwania zwierzęcia: {ex.Message}");
        }
    }

}