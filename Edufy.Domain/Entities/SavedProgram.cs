namespace Edufy.Domain.Entities;

public class SavedProgram
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public int ProgramId { get; set; }
    public DateTime SavedAt { get; set; }

    public User User { get; set; } = null!;
    public Program Program { get; set; } = null!;
}
