namespace Family_and_Spa_Wellness.Services;

/// <summary>
/// Placeholder for the payment processor integration (e.g. Stripe/Square).
/// Not yet implemented — method stubs correspond to the payment provider
/// integration stories in the backlog.
/// </summary>
public interface IPaymentProvider
{
    /// <summary>
    /// US-901: Securely receive a payment for a booking or checkout.
    /// </summary>
    Task<string> ChargeAsync(decimal amount, string currency, string paymentMethodToken);

    /// <summary>
    /// US-902: Check the status of a previously submitted transaction.
    /// </summary>
    Task<string> GetTransactionStatusAsync(string transactionId);

    /// <summary>
    /// US-903: Issue a full or partial refund for a completed transaction.
    /// </summary>
    Task<string> RefundAsync(string transactionId, decimal? amount = null);
}
