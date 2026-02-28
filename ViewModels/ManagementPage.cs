using ClosedXML.Excel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        _ = LoadMonthAsync(CurrentMonth);
    }

    public async Task LoadMonthAsync(DateTime month)
    {
        try
        {
            Days.Clear();

            var firstDay = new DateTime(month.Year, month.Month, 1);
            var nextMonth = firstDay.AddMonths(1);

            var sessions = await _repository.GetWorkSessionsAsync(firstDay, nextMonth);

            var daysInMonth = DateTime.DaysInMonth(month.Year, month.Month);

            for (int day = 1; day <= daysInMonth; day++)
            {
                var date = new DateTime(month.Year, month.Month, day);

                var daySessions = sessions
                    .Where(s => s.StartTime.Date == date)
                    .ToList();

                Days.Add(new CalendarDay
                {
                    Date = date,
                    Sessions = daySessions
                });
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Erro ao carregar mês: {ex.Message}");
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

    [RelayCommand]
    public async Task ShowMonthSummary()
    {
        if (Days == null || !Days.Any(d => d.HasSessions))
        {
            await App.Current.MainPage.DisplayAlert("Resumo do mês",
                "Não há sessões neste mês.", "OK");
            return;
        }

        // Texto do resumo
        var summaries = new List<string>();
        foreach (var day in Days.Where(d => d.HasSessions))
        {
            var totalDay = day.Sessions.Sum(s => s.TotalValue);
            summaries.Add($"{day.Date:dd MMM}: € {totalDay:F2}");
        }

        var totalMonth = Days.Sum(d => d.TotalValue);
        string message = string.Join("\n", summaries) + $"\n\nTotal do mês: € {totalMonth:F2}";

        // Modal com botão exportar
        bool export = await App.Current.MainPage.DisplayAlert(
            $"Resumo {CurrentMonth:MMMM yyyy}",
            message,
            "Exportar",
            "Fechar");

        if (!export) return;

        var option = await App.Current.MainPage.DisplayActionSheet(
            "Exportar para:",
            "Cancelar",
            null,
            "WhatsApp",
            "PDF",
            "Excel");

        switch (option)
        {
            case "WhatsApp":
                await ExportToWhatsApp(message);
                break;
            case "PDF":
                await ExportToPdf(message);
                break;
            case "Excel":
                await ExportToExcel();
                break;
        }
    }

    // -----------------------------
    // Exportações
    // -----------------------------

    private async Task ExportToWhatsApp(string message)
    {
        try
        {
            var url = $"https://api.whatsapp.com/send?text={Uri.EscapeDataString(message)}";
            await Launcher.OpenAsync(url);
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    private async Task ExportToPdf(string message)
    {
        try
        {
            using var document = new PdfDocument();
            var page = document.Pages.Add();

            // Usar Syncfusion.Drawing.PointF
            var location = new Syncfusion.Drawing.PointF(10, 10);

            page.Graphics.DrawString(
                message,
                new PdfStandardFont(PdfFontFamily.Helvetica, 12),
                PdfBrushes.Black,
                location
            );

            string fileName = $"Resumo_{CurrentMonth:yyyy_MM}.pdf";
            string path = Path.Combine(FileSystem.CacheDirectory, fileName);

            using var stream = File.OpenWrite(path);
            document.Save(stream);

            await Launcher.OpenAsync(new Uri(path));

            document.Close(true);
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Erro PDF", ex.Message, "OK");
        }
    }

    private async Task ExportToExcel()
    {
        try
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Resumo");

            ws.Cell(1, 1).Value = "Data";
            ws.Cell(1, 2).Value = "Total (€)";

            int row = 2;
            foreach (var day in Days.Where(d => d.HasSessions))
            {
                ws.Cell(row, 1).Value = day.Date.ToString("dd MMM yyyy");
                ws.Cell(row, 2).Value = day.TotalValue;
                row++;
            }

            ws.Cell(row, 1).Value = "Total do mês";
            ws.Cell(row, 2).Value = Days.Sum(d => d.TotalValue);

            string fileName = $"Resumo_{CurrentMonth:yyyy_MM}.xlsx";
            string path = Path.Combine(FileSystem.CacheDirectory, fileName);

            workbook.SaveAs(path);

            await Launcher.OpenAsync(new Uri(path));
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Erro Excel", ex.Message, "OK");
        }
    }
}