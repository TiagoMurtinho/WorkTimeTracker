namespace WorkTimeTracker.Models;

public class CalendarDay
{
    public DateTime Date { get; set; }
    public bool HasSessions => Sessions?.Any() ?? false;
    public List<WorkSession>? Sessions { get; set; } = [];
    public decimal TotalValue => Sessions?.Sum(s => s.TotalValue) ?? 0;
}
