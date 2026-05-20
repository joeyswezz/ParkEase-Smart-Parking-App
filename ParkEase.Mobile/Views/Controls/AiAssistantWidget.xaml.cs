using ParkEase.Mobile.Services;

namespace ParkEase.Mobile.Views.Controls;

public partial class AiAssistantWidget : ContentView
{
    private readonly ApiService _apiService = new();

    public AiAssistantWidget()
    {
        InitializeComponent();
    }

    private async void FloatingButton_Tapped(object sender, TappedEventArgs e)
    {
        BlurOverlay.IsVisible = true;
        ChatPanel.IsVisible = true;

        BlurOverlay.Opacity = 0;
        ChatPanel.Opacity = 0;
        ChatPanel.TranslationY = 24;
        ChatPanel.Scale = 0.96;

        await Task.WhenAll(
            BlurOverlay.FadeToAsync(1, 220, Easing.CubicOut),
            ChatPanel.FadeToAsync(1, 260, Easing.CubicOut),
            ChatPanel.TranslateToAsync(0, 0, 260, Easing.CubicOut),
            ChatPanel.ScaleToAsync(1, 260, Easing.CubicOut)
        );
    }

private void SuggestedCheapest_Clicked(object sender, EventArgs e)
{
    QuestionEditor.Text = "Which parking lot is the cheapest and currently available?";
}

private void SuggestedAvailable_Clicked(object sender, EventArgs e)
{
    QuestionEditor.Text = "Which parking lot has the most available spaces right now?";
}

    private async void CloseButton_Clicked(object sender, EventArgs e)
    {
        await Task.WhenAll(
            BlurOverlay.FadeToAsync(0, 180, Easing.CubicIn),
            ChatPanel.FadeToAsync(0, 180, Easing.CubicIn),
            ChatPanel.TranslateToAsync(0, 24, 180, Easing.CubicIn),
            ChatPanel.ScaleToAsync(0.96, 180, Easing.CubicIn)
        );

        ChatPanel.IsVisible = false;
        BlurOverlay.IsVisible = false;
    }

    private async void AskButton_Clicked(object sender, EventArgs e)
    {
        var question = QuestionEditor.Text;

        if (string.IsNullOrWhiteSpace(question))
        {
            AnswerLabel.Text = "Please enter a question first.";
            return;
        }

        try
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;
            AnswerLabel.Text = "Thinking...";

            var answer = await _apiService.AskParkingAssistantAsync(question);
            AnswerLabel.Text = answer;
        }
        catch (Exception ex)
        {
            AnswerLabel.Text = $"AI assistant error: {ex.Message}";
        }
        finally
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
        }
    }
}