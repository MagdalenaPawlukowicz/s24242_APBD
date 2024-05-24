using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zadanie5.Context;
using Zadanie5.DTOs;
using Zadanie5.Models;

namespace Zadanie5.Controllers;

[Route("api/trips")]
[ApiController]
public class TripController(Zadanie5Context context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TripResponseDTO>>> GetTrips()
    {
        // Pobranie listy podróży posortowanej malejąco po dacie rozpoczęcia
        var trips = await context.Trips
            .OrderByDescending(t => t.DateFrom)
            .Include(t => t.IdCountries)
            .Include(t => t.ClientTrips)
            .ThenInclude(ct => ct.IdClientNavigation)
            .ToListAsync();

        if (trips == null || trips.Count == 0)
        {
            return NotFound();
        }

        var formattedTrips = trips.Select(t => new TripResponseDTO
        {
            Name = t.Name,
            Description = t.Description,
            DateFrom = t.DateFrom.ToString("yyyy-MM-dd"), // Format daty
            DateTo = t.DateTo.ToString("yyyy-MM-dd"), // Format daty
            MaxPeople = t.MaxPeople,
            Countries = t.IdCountries.Select(c => new CountryDTO
            {
                Name = c.Name
            }).ToList(),
            Clients = t.ClientTrips.Select(ct => new ClientDTO
            {
                FirstName = ct.IdClientNavigation.FirstName,
                LastName = ct.IdClientNavigation.LastName
            }).ToList()
        }).ToList();

        return Ok(formattedTrips);
    }
    
    [HttpDelete("api/clients/{idClient}")]
    public async Task<IActionResult> DeleteClient(int idClient)
    {
        var client = await context.Clients
            .Include(c => c.ClientTrips)
            .FirstOrDefaultAsync(c => c.IdClient == idClient);

        if (client == null)
        {
            return NotFound();
        }

        if (client.ClientTrips.Any())
        {
            return BadRequest("Client cannot be deleted because they are associated with one or more trips.");
        }

        context.Clients.Remove(client);
        await context.SaveChangesAsync();

        return NoContent();
    }
    
    
        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> AssignClientToTrip(int idTrip, [FromBody] AssignClientToTripRequest request)
        {
            // Check if the trip exists
            var trip = await context.Trips
                .Include(t => t.ClientTrips)
                .FirstOrDefaultAsync(t => t.IdTrip == idTrip);

            if (trip == null)
            {
                return NotFound(new { message = "Trip not found." });
            }

            // Check if the client already exists
            var client = await context.Clients
                .FirstOrDefaultAsync(c => c.Pesel == request.Pesel);

            if (client == null)
            {
                // Add the new client to the database
                client = new Client
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Telephone = request.Telephone,
                    Pesel = request.Pesel
                };
                context.Clients.Add(client);
                await context.SaveChangesAsync();
            }

            // Check if the client is already assigned to the trip
            var clientTrip = await context.ClientTrips
                .FirstOrDefaultAsync(ct => ct.IdClient == client.IdClient && ct.IdTrip == idTrip);

            if (clientTrip != null)
            {
                return BadRequest(new { message = "Client is already assigned to this trip." });
            }

            // Assign the client to the trip
            clientTrip = new ClientTrip
            {
                IdClient = client.IdClient,
                IdTrip = trip.IdTrip,
                RegisteredAt = DateTime.Now,
                PaymentDate = request.PaymentDate
            };

            context.ClientTrips.Add(clientTrip);
            await context.SaveChangesAsync();

            return Ok(new { message = "Client assigned to trip successfully." });
        }


}