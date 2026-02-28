using WorkTimeTracker.Models;
using WorkTimeTracker.ViewModels;

namespace WorkTimeTracker.Views;

public partial class ManagementPage : ContentPage
{
    public ManagementPage(ManagementPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;

        CalendarCollectionView.SelectionChanged += async (s, e) =>
        {
            if (e.CurrentSelection.FirstOrDefault() is CalendarDay day)
            {
                await vm.ShowDaySummaryCommand.ExecuteAsync(day);
                CalendarCollectionView.SelectedItem = null;
            }
        };
    }
}