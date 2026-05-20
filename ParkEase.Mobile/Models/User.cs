using SQLite;

namespace ParkEase.Mobile.Models;

public class User
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string DisplayName { get; set; } = "";

    public string EmailAddress { get; set; } = "";
    public string PhoneNumber { get; set; } = "";

    public string Password { get; set; } = "";

    public string ProfileImage { get; set; } = "profile_placeholder.png";

    public string Role { get; set; } = "Regular";

    public bool AcceptedTerms { get; set; }
    public bool IsAdult { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.Now;
}