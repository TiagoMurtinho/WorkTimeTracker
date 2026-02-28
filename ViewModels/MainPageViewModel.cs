using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WorkTimeTracker.Data;
using Timer = System.Timers.Timer;

namespace WorkTimeTracker.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    private readonly WorkTimeRepository _workTimeRepository;
    private readonly Timer _timer;
    private DateTime _startTime;

    [ObservableProperty]
    private TimeSpan _elapsedTime;

    [ObservableProperty]
    private string _dateText;

    [ObservableProperty]
    private string _timeText;

    [ObservableProperty]
    private bool _isRunning;

    public DateTime StartTime => _startTime;

    public MainPageViewModel(WorkTimeRepository workTimeRepository)
    {
        _workTimeRepository = workTimeRepository;

        _dateText = DateTime.Now.ToString("dd MMM yyyy");
        _timeText = DateTime.Now.ToString("HH:mm:ss");

        _timer = new Timer(1000);
        _timer.Elapsed += (s, e) =>
        {
            ElapsedTime = DateTime.Now - _startTime;
        };
    }

    [RelayCommand]
    public void ToggleTimer()
    {
        if (IsRunning)
        {
            StopTimer();
            OnTimerStopped?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            StartTimer();
        }

        OnPropertyChanged(nameof(ButtonText));
        OnPropertyChanged(nameof(ButtonColor));
    }

    public string ButtonText => IsRunning ? "STOPPED" : "START";
    public Color ButtonColor => IsRunning ? Colors.Red : Colors.Green;

    public void StartTimer()
    {
        _startTime = DateTime.Now;
        _timer.Start();
        IsRunning = true;
    }

    public void StopTimer()
    {
        _timer.Stop();
        IsRunning = false;
    }

    public void UpdateDateTime()
    {
        DateText = DateTime.Now.ToString("dd MMM yyyy");
        TimeText = DateTime.Now.ToString("HH:mm:ss");
    }

    // Evento que a View escuta para abrir modal
    public event EventHandler? OnTimerStopped;
}