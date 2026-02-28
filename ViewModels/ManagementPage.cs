using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using WorkTimeTracker.Data;
using WorkTimeTracker.Models;

namespace WorkTimeTracker.ViewModels;

public partial class ManagementPageViewModel : ObservableObject
{
    private readonly WorkTimeRepository _repository;

    [ObservableProperty]
    private ObservableCollection<CalendarDay> _days = new();

    [ObservableProperty]
    private DateTime _currentMonth;

    public ManagementPageViewModel(WorkTimeRepository repository)
    {
        _repository = repository;
        CurrentMonth = DateTime.Now;
        LoadMonth(CurrentMonth);
    }

    public async void LoadMonth(DateTime month)
    {
        Days.Clear();

        // Primeiro dia do mês
        var firstDay = new DateTime(month.Year, month.Month, 1);
        var lastDay = firstDay.AddMonths(1).AddDays(-1);

        // Pega todas as sessões do mês
        var sessions = await _repository.GetWorkSessionsAsync(firstDay, lastDay);

        for (int day = 1; day <= lastDay.Day; day++)
        {
            var date = new DateTime(month.Year, month.Month, day);
            var daySessions = sessions.Where(s => s.StartTime.Date == date).ToList();

            Days.Add(new CalendarDay
            {
                Date = date,
                Sessions = daySessions
            });
        }
    }

    // Comando ao clicar em um dia
    [RelayCommand]
    public async Task ShowDaySummary(CalendarDay day)
    {
        if (day.Sessions == null || !day.Sessions.Any())
        {
            await App.Current.MainPage.DisplayAlert("Resumo", "Não há sessões neste dia.", "OK");
            return;
        }

        var summaries = new List<string>();
        foreach (var s in day.Sessions)
        {
            string workTypeName = await _repository.GetWorkTypeNameById(s.WorkTypeId);
            summaries.Add($"{workTypeName}: {s.DurationMinutes / 60.0:F2}h → € {s.TotalValue:F2}");
        }

        decimal total = day.Sessions.Sum(s => s.TotalValue);

        await App.Current.MainPage.DisplayAlert(
            $"Resumo {day.Date:dd MMM yyyy}",
            string.Join("\n", summaries) + $"\n\nTotal: € {total:F2}",
            "OK");
    }
}