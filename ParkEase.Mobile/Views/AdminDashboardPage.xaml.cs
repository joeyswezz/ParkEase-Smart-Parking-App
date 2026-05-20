using ParkEase.Mobile.ViewModels;

namespace ParkEase.Mobile.Views;

public partial class AdminDashboardPage : ContentPage
{
    public AdminDashboardPage()
    {
        InitializeComponent();
    }

    private async void ChooseLotImage_Clicked(object sender, EventArgs e)
    {
        try
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Choose Parking Lot Image"
            });

            if (result is null)
                return;

            var savedPath = await SaveImageToAppDataAsync(result);

            if (BindingContext is AdminDashboardViewModel vm)
                vm.UpdateLotImage(savedPath);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Image Error", ex.Message, "OK");
        }
    }

    private async void TakeLotImage_Clicked(object sender, EventArgs e)
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
                Title = "Take Parking Lot Photo"
            });

            if (result is null)
                return;

            var savedPath = await SaveImageToAppDataAsync(result);

            if (BindingContext is AdminDashboardViewModel vm)
                vm.UpdateLotImage(savedPath);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Camera Error", ex.Message, "OK");
        }
    }

    private static async Task<string> SaveImageToAppDataAsync(FileResult photo)
    {
        var fileName = $"parking_lot_{DateTime.Now:yyyyMMddHHmmss}.jpg";
        var newFilePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

        await using var stream = await photo.OpenReadAsync();
        await using var newStream = File.OpenWrite(newFilePath);

        await stream.CopyToAsync(newStream);

        return newFilePath;
    }
}