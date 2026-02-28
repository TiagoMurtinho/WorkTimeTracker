using System.Diagnostics;
using WorkTimeTracker.Data;
using WorkTimeTracker.ViewModels;

namespace WorkTimeTracker.Views;

public partial class SettingsPage : ContentPage
{
    private readonly SettingsPageViewModel _viewModel;

    public SettingsPage(WorkTimeRepository repository)
    {
        InitializeComponent();
        _viewModel = new SettingsPageViewModel(repository, this);
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadWorkTypesAsync();
    }
}