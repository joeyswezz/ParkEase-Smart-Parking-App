using ParkEase.Mobile.ViewModels;

namespace ParkEase.Mobile.Views;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ProfileViewModel vm)
            vm.LoadProfile();
    }

    private async void ChoosePhoto_Clicked(object sender, EventArgs e)
    {
        try
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Choose Profile Photo"
            });

            if (result is null)
                return;

            var savedPath = await SaveImageToAppDataAsync(result);

            if (BindingContext is ProfileViewModel vm)
                vm.UpdateProfileImage(savedPath);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Image Error", ex.Message, "OK");
        }
    }

    private async void TakePhoto_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (!MediaPicker.Default.IsCaptureSupported)
            {
                await DisplayAlert("Camera Not Available", "This device does not support camera capture.", "OK");
                return;
            }

            var result = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
            {
                Title = "Take Profile Photo"
            });

            if (result is null)
                return;

            var savedPath = await SaveImageToAppDataAsync(result);

            if (BindingContext is ProfileViewModel vm)
                vm.UpdateProfileImage(savedPath);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Camera Error", ex.Message, "OK");
        }
    }

    private static async Task<string> SaveImageToAppDataAsync(FileResult photo)
    {
        var fileName = $"profile_{DateTime.Now:yyyyMMddHHmmss}.jpg";
        var newFilePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

        await using var stream = await photo.OpenReadAsync();
        await using var newStream = File.OpenWrite(newFilePath);

        await stream.CopyToAsync(newStream);

        return newFilePath;
    }
}