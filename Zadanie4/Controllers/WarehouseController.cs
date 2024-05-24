using Microsoft.AspNetCore.Mvc;
using Zadanie4.Models;
using Zadanie4.Repositories;

namespace Zadanie4.Controllers;

[Route("api/warehouse")]
[ApiController]
public class WarehouseController : ControllerBase
{
    private IWarehouseRepository _warehouseRepository;
    
    public WarehouseController(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    [HttpPost]
    public async Task<ActionResult<int>> AddProductToWarehouse([FromBody] WarehouseRequestModel request)
    {
        try
        {
            // ad. 1
            var product = await _warehouseRepository.GetProductByIdAsync(request.IdProduct);
            var warehouse = await _warehouseRepository.GetWarehouseByIdAsync(request.IdWarehouse);
            
            if (product == null)
                return NotFound("Product not found.");
            
            if (warehouse == null)
                return NotFound("Warehouse not found.");
            
            if (request.Amount <= 0)
                return BadRequest("Amount should be greater than 0.");

            // ad. 2
            var order = await _warehouseRepository.GetOrderAsync(request.IdProduct, request.Amount, request.CreatedAt);
            if (order == null)
                return NotFound("Order not found or order's creation date is later than provided date.");
            
            // ad. 3
            if (order.FulfilledAt != null)
                return Conflict("Order has already been fulfilled.");

            // ad. 4 
            order.FulfilledAt = DateTime.Now;
            await _warehouseRepository.UpdateOrderAsync(order);

            // ad. 5 
            var price = product.Price * request.Amount;
            var productWarehouse = new ProductWarehouse
            {
                IdWarehouse = request.IdWarehouse,
                IdProduct = request.IdProduct,
                IdOrder = order.IdOrder,
                Amount = request.Amount,
                Price = price,
                CreatedAt = DateTime.Now
            };
            var id = await _warehouseRepository.AddProductToWarehouseAsync(productWarehouse);

            // ad. 6
            return Ok(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, $"Error adding product to warehouse: {ex.Message}");
        }
    }

    [HttpPost("procedure")]
    public async Task<ActionResult<int>> AddProductToWarehouseWithStoredProcedure([FromBody] WarehouseRequestModel request)
    {
        try
        {
             var id = await _warehouseRepository.AddProductToWarehouseByStoredProcedureAsync(request.IdProduct,
                request.IdWarehouse, request.Amount, request.CreatedAt);
            
            return Ok(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, $"Error adding product to warehouse: {ex.Message}");
        }
    }
}