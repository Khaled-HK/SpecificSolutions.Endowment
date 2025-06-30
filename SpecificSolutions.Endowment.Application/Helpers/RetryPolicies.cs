using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;

namespace SpecificSolutions.Endowment.Application.Helpers
{
    public static class RetryPolicies
    {
        public static AsyncRetryPolicy DbConcurrencyRetryPolicy => Policy
            .Handle<DbUpdateConcurrencyException>() // Handle concurrency exceptions
            .WaitAndRetryAsync(
                retryCount: 3, // Retry 3 times
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // Exponential backoff
                onRetry: (exception, retryCount, context) =>
                {
                    // Log retry attempts
                    Console.WriteLine($"Retry {retryCount} due to concurrency conflict: {exception.Message}");
                });
    }
}
