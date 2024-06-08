using Kolokwium_s24242.Context;
using Kolokwium_s24242.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium_s24242.Controllers;


[Route("/api/[controller]")]
[ApiController]
public class ShopController : ControllerBase
{
    private readonly ApbdContext _context;

    public ShopController(ApbdContext context)
    {
        _context = context;
    }
    
    [HttpGet("{idClient}")]
    public async Task<IActionResult> GetClientWithSubscriptions(int idClient)
    {
        var clientData = await _context.Clients
            .Where(c => c.IdClient == idClient)
            .Select(c => new ClientDto
            {
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                Phone = c.Phone,
                Subscriptions = c.Payments
                    .GroupBy(p => p.Subscription)
                    .Select(g => new SubscriptionDto
                    {
                        IdSubscription = g.Key.IdSubscription,
                        Name = g.Key.Name,
                        TotalPaidAmount = g.Sum(p => p.Subscription.Price)
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();

        if (clientData == null)
        {
            return NotFound();
        }

        return Ok(clientData);
    }

}


public class SubscriptionDto
{
    public int IdSubscription { get; set; }
    public string Name { get; set; }
    public decimal TotalPaidAmount { get; set; }
}

public class ClientDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public List<SubscriptionDto> Subscriptions { get; set; }
}

public class PaymentRequestDto
{
    public int IdClient { get; set; }
    public int IdSubscription { get; set; }
    public decimal Payment { get; set; }
}