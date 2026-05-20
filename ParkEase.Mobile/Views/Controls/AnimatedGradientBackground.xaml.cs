namespace ParkEase.Mobile.Views.Controls;

public partial class AnimatedGradientBackground : ContentView
{
    private bool _isAnimating;

    public AnimatedGradientBackground()
    {
        InitializeComponent();

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    private void OnUnloaded(object? sender, EventArgs e)
    {
        _isAnimating = false;
        this.AbortAnimation("GradientLoop");
    }

    private async void OnLoaded(object? sender, EventArgs e)
    {
        await Task.Delay(500);

        if (_isAnimating)
            return;

        _isAnimating = true;

        _ = StartAnimationAsync();
    }

    private async Task StartAnimationAsync()
    {
        try
        {
            BlobOne.TranslationX = -120;
            BlobOne.TranslationY = -120;

            BlobTwo.TranslationX = 120;
            BlobTwo.TranslationY = -40;

            BlobThree.TranslationX = -100;
            BlobThree.TranslationY = 120;

            BlobFour.TranslationX = 100;
            BlobFour.TranslationY = 100;

            while (_isAnimating)
            {
                await BlobOne.TranslateTo(80, 60, 5000, Easing.SinInOut);
                if (!_isAnimating) break;

                await BlobTwo.TranslateTo(-80, 90, 5000, Easing.SinInOut);
                if (!_isAnimating) break;

                await BlobThree.TranslateTo(90, -80, 5000, Easing.SinInOut);
                if (!_isAnimating) break;

                await BlobFour.TranslateTo(-80, -60, 5000, Easing.SinInOut);
                if (!_isAnimating) break;

                await BlobOne.TranslateTo(-120, -120, 5000, Easing.SinInOut);
                await BlobTwo.TranslateTo(120, -40, 5000, Easing.SinInOut);
                await BlobThree.TranslateTo(-100, 120, 5000, Easing.SinInOut);
                await BlobFour.TranslateTo(100, 100, 5000, Easing.SinInOut);
            }
        }
        catch
        {
            _isAnimating = false;
        }
    }
}