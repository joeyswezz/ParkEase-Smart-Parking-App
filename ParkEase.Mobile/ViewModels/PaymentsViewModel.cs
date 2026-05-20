using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ParkEase.Mobile.Data;
using ParkEase.Mobile.Models;
using System.Collections.ObjectModel;

namespace ParkEase.Mobile.ViewModels;

public partial class PaymentsViewModel : ObservableObject
{
    private readonly PaymentDatabaseService _paymentDb = new();

    public ObservableCollection<PaymentCard> Cards { get; } = new();

    [ObservableProperty]
    private string cardholderName = "";

    [ObservableProperty]
    private string cardNumber = "";

    [ObservableProperty]
    private string expiryDate = "";

    [ObservableProperty]
    private string cvv = "";

    [ObservableProperty]
    private string selectedCardType = "Visa";

    public ObservableCollection<string> CardTypes { get; } = new()
    {
        "Visa",
        "Mastercard",
        "American Express"
    };

    public PaymentsViewModel()
    {
        _ = LoadCardsAsync();
    }

    [RelayCommand]
    private async Task LoadCardsAsync()
    {
        Cards.Clear();

        var cards = await _paymentDb.GetCardsAsync();

        foreach (var card in cards)
            Cards.Add(card);
    }

    [RelayCommand]
    private async Task SaveCardAsync()
    {
        if (string.IsNullOrWhiteSpace(CardholderName) ||
            string.IsNullOrWhiteSpace(CardNumber))
            return;

        var card = new PaymentCard
        {
            CardholderName = CardholderName,
            CardNumber = CardNumber,
            ExpiryDate = ExpiryDate,
            CVV = Cvv,
            CardType = SelectedCardType
        };

        await _paymentDb.SaveCardAsync(card);
        await LoadCardsAsync();

        CardholderName = "";
        CardNumber = "";
        ExpiryDate = "";
        Cvv = "";
        SelectedCardType = "Visa";
    }

    [RelayCommand]
    private async Task DeleteCardAsync(PaymentCard card)
    {
        if (card is null)
            return;

        await _paymentDb.DeleteCardAsync(card);
        await LoadCardsAsync();
    }
}