using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ParkEase.Mobile.Services;

namespace ParkEase.Mobile.ViewModels;

public partial class AiAssistantViewModel : ObservableObject
{
    private readonly ApiService _apiService = new();

    [ObservableProperty]
    private string userQuestion = "";

    [ObservableProperty]
    private string aiAnswer = "Ask me which parking lot is best for your trip.";

    [ObservableProperty]
    private bool isBusy;

    [RelayCommand]
    private async Task AskAssistantAsync()
    {
        if (IsBusy)
            return;

        if (string.IsNullOrWhiteSpace(UserQuestion))
        {
            AiAnswer = "Please enter a parking question first.";
            return;
        }

        try
        {
            IsBusy = true;
            AiAnswer = "Thinking...";

            var answer = await _apiService.AskParkingAssistantAsync(UserQuestion);

            AiAnswer = string.IsNullOrWhiteSpace(answer)
                ? "No AI answer was returned."
                : answer;
        }
        catch (Exception ex)
        {
            AiAnswer = $"Something went wrong: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }
}