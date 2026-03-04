namespace Edufy.Domain.Entities;

public class Academy
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? LogoUrl { get; set; }
    public string? About { get; set; }
    public int TotalApplications { get; set; }
    public int TotalStudents { get; set; }
    public decimal GraduationRate { get; set; }   // e.g. 68.00

    public ICollection<Program> Programs { get; set; } = [];
}