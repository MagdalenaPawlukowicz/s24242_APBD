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

[HttpPost]
public async Task<IActionResult> CreatePayment([FromBody] PaymentRequestDto request)
{
    
    var client = await _context.Clients.FindAsync(request.IdClient);
    if (client == null)
    {
        return NotFound("Klient nie istnieje");
    }

    var subscription = await _context.Subscriptions.FindAsync(request.IdSubscription);
    if (subscription == null)
    {
        return NotFound("Subskrypcja nie istnieje.");
    }

    
    if (subscription.EndTime < DateTime.UtcNow)
    {
        return BadRequest("Subskrypcja nie aktywna.");
    }

    
    var lastSale = await _context.Sales
        .Where(s => s.IdClient == request.IdClient && s.IdSubscription == request.IdSubscription)
        .OrderByDescending(s => s.CreatedAt)
        .FirstOrDefaultAsync();

    if (lastSale == null)
    {
        return BadRequest("brak rekordu sprzedaży dla tej subskrypcjii");
    }
    
    var nextPaymentStartDate = lastSale.CreatedAt.AddMonths(subscription.RenewalPeriod);
    
    var existingPayment = await _context.Payments
        .Where(p => p.IdClient == request.IdClient && p.IdSubscription == request.IdSubscription && p.Date >= nextPaymentStartDate)
        .FirstOrDefaultAsync();

    if (existingPayment != null)
    {
        return BadRequest("Płatność za ten okres została  dokonana");
    }
    
    decimal finalPaymentAmount = subscription.Price;

    var highestDiscount = await _context.Discounts
        .Where(d => d.IdSubscription == request.IdSubscription && d.DateFrom <= DateTime.UtcNow && d.DateTo >= DateTime.UtcNow)
        .OrderByDescending(d => d.Value)
        .FirstOrDefaultAsync();

    if (highestDiscount != null)
    {
        finalPaymentAmount -= finalPaymentAmount * (highestDiscount.Value / 100.0m);
    }
    
    if (request.Payment != finalPaymentAmount)
    {
        return BadRequest($"Nieprawidłowa płatność. Powinno być: {finalPaymentAmount}");
    }
    
    var payment = new Payment
    {
        Date = DateTime.UtcNow,
        IdClient = request.IdClient,
        IdSubscription = request.IdSubscription
    };

    _context.Payments.Add(payment);
    await _context.SaveChangesAsync();
    
    return Ok(new { IdPayment = payment.IdPayment });
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