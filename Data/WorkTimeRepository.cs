using SQLite;
using WorkTimeTracker.Models;

namespace WorkTimeTracker.Data;

public class WorkTimeRepository
{
    private readonly SQLiteAsyncConnection _database;

    public WorkTimeRepository(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);

        // Criação das tabelas
        _database.CreateTableAsync<WorkType>().Wait();
        _database.CreateTableAsync<WorkSession>().Wait();
        _database.CreateTableAsync<MonthlyReport>().Wait();
    }

    // -------------------------
    // WorkType CRUD
    // -------------------------
    public Task<List<WorkType>> GetWorkTypesAsync() => _database.Table<WorkType>().ToListAsync();
    public Task<int> AddWorkTypeAsync(WorkType type) => _database.InsertAsync(type);
    public Task<int> UpdateWorkTypeAsync(WorkType type) => _database.UpdateAsync(type);
    public Task<int> DeleteWorkTypeAsync(WorkType type) => _database.DeleteAsync(type);

    // -------------------------
    // WorkSession CRUD
    // -------------------------
    public Task<List<WorkSession>> GetWorkSessionsAsync() => _database.Table<WorkSession>().ToListAsync();
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