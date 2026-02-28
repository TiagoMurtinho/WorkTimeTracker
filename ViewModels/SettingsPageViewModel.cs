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
        string title = workType == null ? "Criar WorkType" : "Editar WorkType";
        string currentName = workType?.Name ?? "";

        string? result = await _page.DisplayPromptAsync(
            title,
            "Digite o nome:",
            initialValue: currentName,
            maxLength: 50,
            keyboard: Keyboard.Text);

        if (string.IsNullOrWhiteSpace(result))
            return;

        if (workType == null)
        {
            var newType = new WorkType { Name = result };
            await _repository.AddWorkTypeAsync(newType);

            var inserted = await _repository.GetWorkTypesAsync();
            newType.Id = inserted.Last().Id;

            WorkTypes.Add(newType);
        }
        else
        {
            workType.Name = result;
            await _repository.UpdateWorkTypeAsync(workType);

            int index = WorkTypes.IndexOf(workType);
            if (index >= 0)
                WorkTypes[index] = workType;
        }
    }
}