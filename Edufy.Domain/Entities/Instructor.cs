namespace Edufy.Domain.Entities;

public class Instructor
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Specialization { get; set; } = null!;
    public string? Bio { get; set; }
    public string? PhotoUrl { get; set; }
    public decimal PriceAzn { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }

    public ICollection<Program> Programs { get; set; } = [];
}