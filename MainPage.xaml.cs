using WorkTimeTracker.ViewModels;

namespace WorkTimeTracker.Views;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        // Atualiza hora e data
        Dispatcher.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            viewModel.UpdateDateTime(); // método que atualiza Data/Hora no ViewModel
            return true;
        });
    }
}
