using Edufy.Domain.Enums;

namespace Edufy.Domain.Entities;

public class Program
{
    public int Id { get; set; }
    public int AcademyId { get; set; }
    public int InstructorId { get; set; }
    public string Name { get; set; } = null!;
    public string? About { get; set; }
    public int DurationMonths { get; set; }
    public int GroupMinSize { get; set; }
    public int GroupMaxSize { get; set; }
    public ProgramStatus Status { get; set; }

    public Academy Academy { get; set; } = null!;
    public Instructor Instructor { get; set; } = null!;
    public ICollection<Module> Modules { get; set; } = [];
    public ICollection<Application> Applications { get; set; } = [];
    public ICollection<SavedProgram> SavedPrograms { get; set; } = [];
}
