using WorkTimeTracker.Data;
using WorkTimeTracker.ViewModels;

namespace WorkTimeTracker.Views;

public partial class MainFlyoutPage : FlyoutPage
{
    private readonly WorkTimeRepository _workTimeRepository;

    public MainFlyoutPage(MainPageViewModel mainViewModel, WorkTimeRepository workTimeRepository)
    {
        InitializeComponent();
        Detail = new NavigationPage(new MainPage(mainViewModel));
        _workTimeRepository = workTimeRepository;
    }

    // Evento para abrir SettingsPage
    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        IsPresented = false;
        await Detail.Navigation.PushAsync(new SettingsPage(_workTimeRepository));
    }

    // Evento para abrir ManagementPage
    private async void OnManagementClicked(object sender, EventArgs e)
    {
        IsPresented = false;
        await Detail.Navigation.PushAsync(new ManagementPage());
    }
}