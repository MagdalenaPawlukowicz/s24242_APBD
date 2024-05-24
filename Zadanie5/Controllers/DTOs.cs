namespace Zadanie5.DTOs
{
    public class AssignClientToTripRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Pesel { get; set; }

        public string IdTrip { get; set; }

        public string TripName { get; set; }
        public DateTime? PaymentDate { get; set; }
    }
    
    public class TripResponseDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string DateFrom { get; set; } // Formatted as "yyyy-MM-dd"
        public string DateTo { get; set; } // Formatted as "yyyy-MM-dd"
        public int MaxPeople { get; set; }
        public List<CountryDTO> Countries { get; set; }
        public List<ClientDTO> Clients { get; set; }
    }

    public class CountryDTO
    {
        public string Name { get; set; }
    }

    public class ClientDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
