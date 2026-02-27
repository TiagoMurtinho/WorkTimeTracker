using SQLite;


namespace WorkTimeTracker.Models;


public class WorkType
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [NotNull]
    public string Name { get; set; } = string.Empty;
}