using System.Collections.ObjectModel;
using System.Windows.Input;
using WorkTimeTracker.Data;
using WorkTimeTracker.Models;

namespace WorkTimeTracker.ViewModels;

public class SettingsPageViewModel : BaseViewModel
{
    private readonly WorkTimeRepository _repository;
    private readonly Page _page;

    public ObservableCollection<WorkType> WorkTypes { get; } = new ObservableCollection<WorkType>();

    public ICommand AddCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand DeleteCommand { get; }

    public SettingsPageViewModel(WorkTimeRepository repository, Page page)
    {
        _repository = repository;
        _page = page;

        AddCommand = new Command(async () =>
        {
            await ShowWorkTypeModal(null);
        });

        EditCommand = new Command<WorkType>(async (workType) =>
        {
            await ShowWorkTypeModal(workType);
        });

        DeleteCommand = new Command<WorkType>(async (workType) =>
        {
            bool confirm = await page.DisplayAlert(
                "Confirmação",
                $"Deseja eliminar '{workType.Name}'?",
                "Sim", "Não");

            if (confirm)
            {
                await _repository.DeleteWorkTypeAsync(workType);
                WorkTypes.Remove(workType);
            }
        });
    }

    public async Task LoadWorkTypesAsync()
    {
        List<WorkType> types = await _repository.GetWorkTypesAsync();
        WorkTypes.Clear();
        foreach (var type in types)
            WorkTypes.Add(type);
    }

    private async Task ShowWorkTypeModal(WorkType? workType)
    {
        bool isNew = workType == null;

        string title = isNew ? "Criar trabalho" : "Editar trabalho";

        // NOME
        string? name = await _page.DisplayPromptAsync(
            title,
            "Nome do trabalho:",
            initialValue: workType?.Name ?? "",
            maxLength: 50,
            keyboard: Keyboard.Text);

        if (string.IsNullOrWhiteSpace(name))
            return;

        // SALÁRIO
        string? rateText = await _page.DisplayPromptAsync(
            title,
            "Salário por hora (€):",
            initialValue: workType?.HourlyRate.ToString("F2") ?? "0.00",
            keyboard: Keyboard.Numeric);

        if (string.IsNullOrWhiteSpace(rateText))
            return;

        if (!decimal.TryParse(rateText.Replace(',', '.'),
            System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture,
            out decimal hourlyRate))
        {
            await _page.DisplayAlert("Erro", "Salário inválido.", "OK");
            return;
        }

        if (isNew)
        {
            var newType = new WorkType
            {
                Name = name,
                HourlyRate = hourlyRate
            };

            await _repository.AddWorkTypeAsync(newType);
            WorkTypes.Add(newType);
        }
        else
        {
            workType!.Name = name;
            workType.HourlyRate = hourlyRate;

            await _repository.UpdateWorkTypeAsync(workType);

            int index = WorkTypes.IndexOf(workType);
            if (index >= 0)
                WorkTypes[index] = workType;
        }
    }
}