using SQLite;

namespace WorkTimeTracker.Models;

public class WorkSession
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [NotNull]
    public DateTime StartTime { get; set; }

    [NotNull]
    public DateTime EndTime { get; set; }

    [NotNull]
    public int DurationMinutes { get; set; }

    [NotNull]
    public int WorkTypeId { get; set; }
}