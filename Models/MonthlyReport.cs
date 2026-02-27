using SQLite;

namespace WorkTimeTracker.Models;

public class MonthlyReport
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [NotNull]
    public int Month { get; set; }

    [NotNull]
    public int Year { get; set; }

    public string? MessageText { get; set; }

    public DateTime? SentAt { get; set; }
}