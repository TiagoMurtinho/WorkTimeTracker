using SQLite;
using WorkTimeTracker.Models;

namespace WorkTimeTracker.Data;

public class WorkTimeRepository
{
    private readonly SQLiteAsyncConnection _database;

    public WorkTimeRepository(string dbPath)
    {
    #if DEBUG
        if (File.Exists(dbPath))
            File.Delete(dbPath);
    #endif

        _database = new SQLiteAsyncConnection(dbPath);

        // Criação das tabelas
        _database.CreateTableAsync<WorkType>().Wait();
        _database.CreateTableAsync<WorkSession>().Wait();
        _database.CreateTableAsync<MonthlyReport>().Wait();
    }

    // -------------------------
    // WorkType CRUD
    // -------------------------
    public async Task<string> GetWorkTypeNameById(int id)
    {
        WorkType type = await _database.Table<WorkType>()
                                   .Where(wt => wt.Id == id)
                                   .FirstOrDefaultAsync();
        return type?.Name ?? "Desconhecido";
    }
    public Task<List<WorkType>> GetWorkTypesAsync() => _database.Table<WorkType>().ToListAsync();
    public Task<int> AddWorkTypeAsync(WorkType type) => _database.InsertAsync(type);
    public Task<int> UpdateWorkTypeAsync(WorkType type) => _database.UpdateAsync(type);
    public Task<int> DeleteWorkTypeAsync(WorkType type) => _database.DeleteAsync(type);

    // -------------------------
    // WorkSession CRUD
    // -------------------------
    public Task<List<WorkSession>> GetWorkSessionsAsync(DateTime start, DateTime end)
    {
        return _database.Table<WorkSession>()
            .Where(ws => ws.StartTime >= start && ws.StartTime < end)
            .ToListAsync();
    }

    public Task<int> AddWorkSessionAsync(WorkSession session) => _database.InsertAsync(session);

    // -------------------------
    // MonthlyReport CRUD
    // -------------------------
    public Task<List<MonthlyReport>> GetMonthlyReportsAsync() => _database.Table<MonthlyReport>().ToListAsync();
    public Task<int> AddMonthlyReportAsync(MonthlyReport report) => _database.InsertAsync(report);

    // -------------------------
    // Utilitários
    // -------------------------
    public Task<int> GetTotalMinutesForMonthAsync(int month, int year)
    {
        string sql = @"
        SELECT SUM(DurationMinutes) 
        FROM WorkSession 
        WHERE strftime('%m', StartTime) = ? AND strftime('%Y', StartTime) = ?";

        return _database.ExecuteScalarAsync<int>(sql, month.ToString("D2"), year.ToString());
    }
}