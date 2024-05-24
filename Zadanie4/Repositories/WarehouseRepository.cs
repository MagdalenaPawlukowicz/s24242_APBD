using System.Data;
using System.Data.SqlClient;
using Zadanie4.Models;

namespace Zadanie4.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly IConfiguration _configuration;

    public WarehouseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<Product> GetProductByIdAsync(int requestIdProduct)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();

        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT * FROM Product WHERE IdProduct = @IdProduct";
        cmd.Parameters.AddWithValue("@IdProduct", requestIdProduct);

        var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Product
            {
                IdProduct = (int)reader["IdProduct"],
                Name = reader["Name"].ToString(),
                Description = reader["Description"].ToString(),
                Price = (decimal)reader["Price"]
            };
        }

        return null;
    }

    public async Task<Warehouse> GetWarehouseByIdAsync(int requestIdWarehouse)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();

        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT * FROM Warehouse WHERE IdWarehouse = @IdWarehouse";
        cmd.Parameters.AddWithValue("@IdWarehouse", requestIdWarehouse);

        var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Warehouse
            {
                IdWarehouse = (int)reader["IdWarehouse"],
                Name = reader["Name"].ToString(),
                Address = reader["Address"].ToString()
            };
        }

        return null;
    }

    public async Task<Order> GetOrderAsync(int requestIdProduct, int requestAmount, DateTime requestCreatedAt)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();

        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT * FROM \"Order\" " +
                          "WHERE IdProduct = @IdProduct " +
                          "AND Amount = @Amount " +
                          "AND CreatedAt <= @CreatedAt " +
                          "ORDER BY CreatedAt DESC";
        cmd.Parameters.AddWithValue("@IdProduct", requestIdProduct);
        cmd.Parameters.AddWithValue("@Amount", requestAmount);
        cmd.Parameters.AddWithValue("@CreatedAt", requestCreatedAt);

        var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            var orderAsync = new Order
            {
                IdOrder = (int)reader["IdOrder"],
                IdProduct = (int)reader["IdProduct"],
                Amount = (int)reader["Amount"],
                CreatedAt = (DateTime)reader["CreatedAt"]
            };

            var fullFilledAt = reader["FulfilledAt"];
            if (!Convert.IsDBNull(fullFilledAt))
            {
                orderAsync.FulfilledAt = (DateTime)fullFilledAt;
            }

            return orderAsync;
        }

        return null;
    }


    public async Task<int> UpdateOrderAsync(Order order)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();

        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "UPDATE \"Order\" SET FulfilledAt = @FulfilledAt WHERE IdOrder = @IdOrder";
        cmd.Parameters.AddWithValue("@FulfilledAt", order.FulfilledAt);
        cmd.Parameters.AddWithValue("@IdOrder", order.IdOrder);

        var affectedCount = await cmd.ExecuteNonQueryAsync();
        return affectedCount;
    }

    public async Task<int> AddProductToWarehouseAsync(ProductWarehouse productWarehouse)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();

        using var transaction = con.BeginTransaction(); // Rozpoczęcie transakcji
        try
        {
            using var cmd = new SqlCommand();
            cmd.Connection = con;

            cmd.Transaction = transaction; // Przypisanie transakcji do polecenia

            cmd.CommandText =
                "INSERT INTO Product_Warehouse (IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) " +
                "VALUES(@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, @CreatedAt);" +
                "SELECT SCOPE_IDENTITY();";
            cmd.Parameters.AddWithValue("@IdWarehouse", productWarehouse.IdWarehouse);
            cmd.Parameters.AddWithValue("@IdProduct", productWarehouse.IdProduct);
            cmd.Parameters.AddWithValue("@IdOrder", productWarehouse.IdOrder);
            cmd.Parameters.AddWithValue("@Amount", productWarehouse.Amount);
            cmd.Parameters.AddWithValue("@Price", productWarehouse.Price);
            cmd.Parameters.AddWithValue("@CreatedAt", productWarehouse.CreatedAt);

            var generatedId = await cmd.ExecuteScalarAsync();

            transaction.Commit(); // Zatwierdzenie transakcji po pomyślnym wykonaniu operacji

            return Convert.ToInt32(generatedId);
        }
        catch (Exception ex)
        {
            transaction.Rollback(); // Wycofanie transakcji w przypadku błędu
            throw;
        }
    }

    public async Task<int> AddProductToWarehouseByStoredProcedureAsync(int idProduct, int idWarehouse, int amount,
        DateTime createdAt)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        {
            using (SqlCommand command = new SqlCommand("AddProductToWarehouse", con))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@IdProduct", idProduct);
                command.Parameters.AddWithValue("@IdWarehouse", idWarehouse);
                command.Parameters.AddWithValue("@Amount", amount);
                command.Parameters.AddWithValue("@CreatedAt", createdAt);

                SqlParameter outputParameter = new SqlParameter();
                outputParameter.ParameterName = "@GeneratedProductWarehouseId";
                outputParameter.SqlDbType = SqlDbType.Int;
                outputParameter.Direction = ParameterDirection.Output;
                command.Parameters.Add(outputParameter);


                await con.OpenAsync();

                using var transaction = con.BeginTransaction(); // Rozpoczęcie transakcji

                try
                {
                    command.Transaction = transaction; // przypisanie transakcji do polecenia

                    await command.ExecuteNonQueryAsync();

                    int generatedProductId = Convert.ToInt32(outputParameter.Value);

                    transaction.Commit(); // zatwierdzenie transakcji po pomyślnym wykonaniu operacji

                    return generatedProductId;
                }
                catch (Exception ex)
                {
                    transaction.Rollback(); // Wycofanie transakcji gdy błąd
                    throw;
                }
            }
        }
    }
}