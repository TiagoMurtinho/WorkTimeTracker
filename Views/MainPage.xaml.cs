using WorkTimeTracker.Data;
using WorkTimeTracker.Models;
using WorkTimeTracker.ViewModels;

namespace WorkTimeTracker.Views;

public partial class MainPage : ContentPage
{
    private readonly MainPageViewModel _viewModel;
    private readonly WorkTimeRepository _repository;

    public MainPage(MainPageViewModel viewModel, WorkTimeRepository repository)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _repository = repository;

        BindingContext = _viewModel;

        _viewModel.OnTimerStopped += async (s, e) => await ShowWorkSessionModal();
    }

    private async Task ShowWorkSessionModal()
    {
        var workTypes = await _repository.GetWorkTypesAsync();
        if (workTypes.Count == 0)
        {
            await DisplayAlert("Aviso", "Não existem WorkTypes definidos.", "OK");
            return;
        }

        var names = workTypes.Select(w => w.Name).ToArray();
        var selectedName = await DisplayActionSheet("Selecione o trabalho", "Cancelar", null, names);

        if (string.IsNullOrWhiteSpace(selectedName) || selectedName == "Cancelar")
            return;

        var selectedType = workTypes.First(w => w.Name == selectedName);

        var duration = DateTime.Now - _viewModel.StartTime;
        int durationMinutes = (int)duration.TotalMinutes;
        decimal totalValue = (decimal)duration.TotalHours * selectedType.HourlyRate;

        bool confirm = await DisplayAlert(
            "Resumo da sessão",
            $"Trabalho: {selectedType.Name}\nHoras: {duration.TotalHours:F2}\nVai receber: € {totalValue:F2}",
            "Salvar",
            "Cancelar");

        if (!confirm) return;

        await _repository.AddWorkSessionAsync(new WorkSession
        {
            StartTime = _viewModel.StartTime,
            EndTime = DateTime.Now,
            DurationMinutes = durationMinutes,
            TotalValue = totalValue,
            WorkTypeId = selectedType.Id
        });
    }
}
