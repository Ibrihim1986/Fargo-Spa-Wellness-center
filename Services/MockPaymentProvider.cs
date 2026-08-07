namespace Family_and_Spa_Wellness.Services;

/// <summary>
/// Temporary mock implementation of IPaymentProvider used until a real
/// payment gateway (e.g. Stripe/Square) is integrated (see US-901/902/903).
/// Simulates success for all card charges except a token literally equal
/// to "decline-test", so the checkout UI's failure/retry path can be tested.
/// </summary>
public class MockPaymentProvider : IPaymentProvider
{
    public Task<string> ChargeAsync(decimal amount, string currency, string paymentMethodToken)
    {
        if (string.Equals(paymentMethodToken, "decline-test", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Card declined.");
        }

        var transactionId = $"MOCK-{Guid.NewGuid():N}"[..16];
        return Task.FromResult(transactionId);
    }

    public Task<string> GetTransactionStatusAsync(string transactionId)
    {
        return Task.FromResult("Success");
    }

    public Task<string> RefundAsync(string transactionId, decimal? amount = null)
    {
        return Task.FromResult($"REFUND-{Guid.NewGuid():N}"[..16]);
    }
}
