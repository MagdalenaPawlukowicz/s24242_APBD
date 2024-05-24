using Zadanie4.Models;

namespace Zadanie4.Repositories;

public interface IWarehouseRepository
{
    Task<Product> GetProductByIdAsync(int requestIdProduct);
    Task<Warehouse> GetWarehouseByIdAsync(int requestIdProduct);
    Task<Order> GetOrderAsync(int requestIdProduct, int requestAmount, DateTime requestCreatedAt);
    Task<int> UpdateOrderAsync(Order order);
    Task<int> AddProductToWarehouseAsync(ProductWarehouse productWarehouse);

    Task<int> AddProductToWarehouseByStoredProcedureAsync(int idProduct, int idWarehouse, int amount, DateTime createdAt);
}