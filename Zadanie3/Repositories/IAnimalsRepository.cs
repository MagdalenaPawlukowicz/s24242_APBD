using Zadanie3_s24242.Models;

namespace Zadanie3_s24242.Repositories;

public interface IAnimalsRepository
{
    IEnumerable<Animal> GetRepoAnimals();

    int AddAnimal(Animal newAnimal);

    int UpdateAnimal(Animal existingAnimal);
    int DeleteAnimal(int idAnimal);
}