namespace Edufy.Domain.Entities;

public class Module
{
    public int Id { get; set; }
    public int ProgramId { get; set; }
    public int Order { get; set; }
    public string Name { get; set; } = null!;
    public bool IsOpen { get; set; }

    public Program Program { get; set; } = null!;
    public ICollection<Lesson> Lessons { get; set; } = [];
}