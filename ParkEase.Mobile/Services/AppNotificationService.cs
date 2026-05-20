namespace ParkEase.Mobile.Services;

public static class AppNotificationService
{
    public static event Action<string, string>? NotificationRequested;

    public static void Show(string title, string message)
    {
        NotificationRequested?.Invoke(title, message);
    }
}