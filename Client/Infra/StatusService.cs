namespace BlazorPatchDemo.Client.Infra;

public class StatusService
{
    private string _message = string.Empty;
    
    // Components wishing to observe any changes in the Message state
    // should subscribe on this event
    public event Action? OnChange;

    public string Message
    {
        get => _message;
        set
        {
            _message = value;
            NotifyStateChanged();
        }
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
