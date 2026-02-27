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
        var repository = new WorkTimeRepository(Path.Combine(FileSystem.AppDataDirectory, "worktracker.db3"));
        var mainViewModel = new MainPageViewModel(repository);
        return new Window(new MainPage(mainViewModel));
    }
}