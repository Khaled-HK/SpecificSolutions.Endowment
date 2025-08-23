using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace SpecificSolutions.Endowment.Infrastructure.Services
{
    /// <summary>
    /// سياسة إعادة المحاولة للبريد الإلكتروني
    /// </summary>
    public static class EmailRetryPolicy
    {
        /// <summary>
        /// إنشاء سياسة إعادة المحاولة للبريد الإلكتروني
        /// </summary>
        public static AsyncRetryPolicy<bool> CreateRetryPolicy(ILogger logger)
        {
            return Policy<bool>
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt => 
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // Exponential backoff
                    onRetry: (exception, timeSpan, retryCount, context) =>
                    {
                                            logger.LogWarning(
                        "Email sending attempt {RetryCount} failed. Retrying in {Delay}ms. Error: {Error}",
                        retryCount,
                        timeSpan.TotalMilliseconds,
                        exception.ToString());
                    });
        }

        /// <summary>
        /// تنفيذ عملية إرسال البريد مع إعادة المحاولة
        /// </summary>
        public static async Task<bool> ExecuteWithRetryAsync(
            Func<Task<bool>> emailAction,
            ILogger logger,
            string emailAddress)
        {
            var retryPolicy = CreateRetryPolicy(logger);

            return await retryPolicy.ExecuteAsync(async () =>
            {
                logger.LogInformation("Attempting to send email to {EmailAddress}", emailAddress);
                var result = await emailAction();
                
                if (!result)
                {
                    throw new Exception($"Email sending failed for {emailAddress}");
                }
                
                return result;
            });
        }
    }
}
