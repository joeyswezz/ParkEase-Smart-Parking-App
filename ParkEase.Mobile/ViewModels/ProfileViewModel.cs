using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ParkEase.Mobile.Services;

namespace ParkEase.Mobile.ViewModels;

public partial class ProfileViewModel : ObservableObject
{
    [ObservableProperty]
    private string firstName = "";

    [ObservableProperty]
    private string lastName = "";

    [ObservableProperty]
    private string displayName = "";

    [ObservableProperty]
    private string emailAddress = "";

    [ObservableProperty]
    private string phoneNumber = "";

    [ObservableProperty]
private string selectedImagePath = "";

    [ObservableProperty]
    private string profileImage = "profile_placeholder.png";

    [ObservableProperty]
    private string statusMessage = "";

    public ProfileViewModel()
    {
        LoadProfile();
    }

    public void LoadProfile()
    {
        var user = AppState.CurrentUser;

        if (user is null)
        {
            StatusMessage = "No user is currently logged in.";
            return;
        }

        FirstName = user.FirstName;
        LastName = user.LastName;
        DisplayName = user.DisplayName;
        EmailAddress = user.EmailAddress;
        PhoneNumber = user.PhoneNumber;
        ProfileImage = user.ProfileImage;
    }

    public void UpdateProfileImage(string imagePath)
{
    if (string.IsNullOrWhiteSpace(imagePath))
        return;

    ProfileImage = imagePath;
    SelectedImagePath = imagePath;
}

    [RelayCommand]
    private void SaveProfile()
    {
        var user = AppState.CurrentUser;

        if (user is null)
        {
            StatusMessage = "No user is currently logged in.";
            return;
        }

        user.FirstName = FirstName;
        user.LastName = LastName;
        user.DisplayName = DisplayName;
        user.EmailAddress = EmailAddress;
        user.PhoneNumber = PhoneNumber;
        user.ProfileImage = ProfileImage;

        StatusMessage = "Profile updated successfully.";
        AppNotificationService.Show(
    "Profile Updated",
    "Your account details were saved.");
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        AppState.CurrentUser = null;
        await Shell.Current.GoToAsync("//Login");
    }
}