using System.Data.SqlClient;
using Zadanie3_s24242.Models;

namespace Zadanie3_s24242.Repositories;

public class AnimalsRepository :IAnimalsRepository
{
    private readonly IConfiguration _configuration;

    public AnimalsRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IEnumerable<Animal> GetRepoAnimals()
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
        
        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT IdAnimal, Name, Description, CATEGORY, AREA FROM Animal ORDER BY Name";
        
        var dr = cmd.ExecuteReader();
        var animals = new List<Animal>();
        while (dr.Read())
        {
            var animal = new Animal()
            {
                IdAnimal = (int)dr["IdAnimal"],
                Name = dr["Name"].ToString(),
                Description = dr["Description"].ToString(),
                Category = dr["CATEGORY"].ToString(),
                Area = dr["AREA"].ToString(),
            };
            animals.Add(animal);
        }
        
        return animals;
    }
    
    public int AddAnimal(Animal newAnimal)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
            
        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "INSERT INTO Animal (Name, Description, Category, Area) VALUES (@Name, @Description, @Category, @Area)";
            
        cmd.Parameters.AddWithValue("@Name", newAnimal.Name);
        cmd.Parameters.AddWithValue("@Description", newAnimal.Description);
        cmd.Parameters.AddWithValue("@Category", newAnimal.Category);
        cmd.Parameters.AddWithValue("@Area", newAnimal.Area);
            
        var affectedCount = cmd.ExecuteNonQuery();
        return affectedCount;
    }
    

    public int UpdateAnimal(Animal updatedAnimal)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
            
        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "UPDATE Animal SET Name=@Name, Description=@Description, CATEGORY=@Category, AREA=@Area WHERE IdAnimal = @IdAnimal";
        cmd.Parameters.AddWithValue("@IdAnimal", updatedAnimal.IdAnimal);
        cmd.Parameters.AddWithValue("@Name", updatedAnimal.Name);
        cmd.Parameters.AddWithValue("@Description", updatedAnimal.Description);
        cmd.Parameters.AddWithValue("@Category", updatedAnimal.Category);
        cmd.Parameters.AddWithValue("@Area", updatedAnimal.Area);
            
        var affectedCount = cmd.ExecuteNonQuery();
        return affectedCount;
    }
    
    public int DeleteAnimal(int idAnimal)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
            
        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "DELETE FROM Animal WHERE IdAnimal = @IdAnimal";
        cmd.Parameters.AddWithValue("@IdAnimal", idAnimal);
            
        var affectedCount = cmd.ExecuteNonQuery();
        return affectedCount;
    }
}