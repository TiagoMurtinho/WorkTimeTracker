using WorkTimeTracker.Models;
using WorkTimeTracker.ViewModels;

namespace WorkTimeTracker.Views;

public partial class ManagementPage : ContentPage
{
    private readonly ManagementPageViewModel _vm;

    public ManagementPage(ManagementPageViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;

        CalendarCollectionView.SelectionChanged += async (s, e) =>
        {
            if (e.CurrentSelection.FirstOrDefault() is CalendarDay day)
            {
                await vm.ShowDaySummary(day);
                CalendarCollectionView.SelectedItem = null;
            }
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadMonthAsync(_vm.CurrentMonth);
    }
}