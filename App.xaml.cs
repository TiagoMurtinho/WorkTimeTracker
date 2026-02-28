using WorkTimeTracker.Views;
using WorkTimeTracker.ViewModels;
using WorkTimeTracker.Data;

namespace WorkTimeTracker;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        WorkTimeRepository repository = new(Path.Combine(FileSystem.AppDataDirectory, "worktracker.db3"));
        MainPageViewModel mainViewModel = new(repository);
        return new Window(new MainFlyoutPage(mainViewModel, repository));
    }
}