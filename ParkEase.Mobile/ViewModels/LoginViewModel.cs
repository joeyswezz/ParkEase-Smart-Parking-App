using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ParkEase.Mobile.Data;
using ParkEase.Mobile.Models;

namespace ParkEase.Mobile.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly UserDatabaseService _userDb = new();

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
    private string password = "";

    [ObservableProperty]
    private string confirmPassword = "";

    [ObservableProperty]
    private bool acceptedTerms;

    [ObservableProperty]
    private bool isAdult;

    [ObservableProperty]
    private string statusMessage = "";

    [ObservableProperty]
    private bool isRegisterMode;

    [ObservableProperty]
    private string formTitle = "Welcome Back";

    [ObservableProperty]
    private string primaryButtonText = "Login";

    [ObservableProperty]
    private string toggleButtonText = "Create an account";

    [RelayCommand]
    private void ToggleMode()
    {
        IsRegisterMode = !IsRegisterMode;

        if (IsRegisterMode)
        {
            FormTitle = "Create Account";
            PrimaryButtonText = "Register";
            ToggleButtonText = "Already have an account?";
        }
        else
        {
            FormTitle = "Welcome Back";
            PrimaryButtonText = "Login";
            ToggleButtonText = "Create an account";
        }

        StatusMessage = "";
    }

    [RelayCommand]
    private async Task SubmitAsync()
    {
        StatusMessage = "";

        if (string.IsNullOrWhiteSpace(EmailAddress) ||
            string.IsNullOrWhiteSpace(Password))
        {
            StatusMessage = "Email and password are required.";
            return;
        }

        if (IsRegisterMode)
        {
            if (string.IsNullOrWhiteSpace(FirstName) ||
                string.IsNullOrWhiteSpace(LastName) ||
                string.IsNullOrWhiteSpace(DisplayName) ||
                string.IsNullOrWhiteSpace(PhoneNumber))
            {
                StatusMessage = "Please complete all registration fields.";
                return;
            }

            if (Password != ConfirmPassword)
            {
                StatusMessage = "Passwords do not match.";
                return;
            }

            if (!IsAdult)
            {
                StatusMessage = "You must confirm that you are 18 or older.";
                return;
            }

            if (!AcceptedTerms)
            {
                StatusMessage = "You must accept the Terms and Conditions.";
                return;
            }

            var existingUser = await _userDb.GetUserByEmailAsync(EmailAddress);

            if (existingUser is not null)
            {
                StatusMessage = "An account with this email already exists.";
                return;
            }

           var role = "Regular";

if (EmailAddress.Equals("admin@parkease.com", StringComparison.OrdinalIgnoreCase))
    role = "Admin";

if (EmailAddress.Equals("operator@parkease.com", StringComparison.OrdinalIgnoreCase))
    role = "Operator";

var user = new User
{
    FirstName = FirstName,
    LastName = LastName,
    DisplayName = DisplayName,
    EmailAddress = EmailAddress,
    PhoneNumber = PhoneNumber,
    Password = Password,
    AcceptedTerms = AcceptedTerms,
    IsAdult = IsAdult,
    ProfileImage = "profile_placeholder.png",
    Role = role,
    DateCreated = DateTime.Now
};

            await _userDb.RegisterUserAsync(user);

            StatusMessage = "Account created. You can now login.";

            IsRegisterMode = false;
            FormTitle = "Welcome Back";
            PrimaryButtonText = "Login";
            ToggleButtonText = "Create an account";

            Password = "";
            ConfirmPassword = "";

            return;
        }

        var loggedInUser = await _userDb.LoginAsync(EmailAddress, Password);

if (loggedInUser is null)
{
    StatusMessage = "Invalid email or password.";
    return;
}

ParkEase.Mobile.Services.AppState.CurrentUser = loggedInUser;

await Shell.Current.GoToAsync("//Dashboard");
    }
}