using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace SpecificSolutions.Endowment.Infrastructure.Services
{
    /// <summary>
    /// سياسة إعادة المحاولة للبريد الإلكتروني - النسخة المجانية المحسنة
    /// </summary>
    public static class EmailRetryPolicy
    {
        /// <summary>
        /// إنشاء سياسة إعادة المحاولة للبريد الإلكتروني
        /// </summary>
        public static AsyncRetryPolicy<bool> CreateRetryPolicy(ILogger logger, int retryAttempts = 3, int retryDelaySeconds = 2)
        {
            return Policy<bool>
                .Handle<Exception>()
                .OrResult(result => result == false) // Retry on false result as well
                .WaitAndRetryAsync(
                    retryCount: retryAttempts,
                    sleepDurationProvider: retryAttempt => 
                        TimeSpan.FromSeconds(retryDelaySeconds * Math.Pow(2, retryAttempt - 1)), // Exponential backoff
                    onRetry: (outcome, timeSpan, retryCount, context) =>
                    {
                        if (outcome.Exception != null)
                        {
                            logger.LogWarning(
                                "Email sending attempt {RetryCount} failed. Retrying in {Delay}ms. Error: {Error}",
                                retryCount,
                                timeSpan.TotalMilliseconds,
                                outcome.Exception.ToString());
                        }
                        else
                        {
                            logger.LogWarning(
                                "Email sending attempt {RetryCount} returned false. Retrying in {Delay}ms.",
                                retryCount,
                                timeSpan.TotalMilliseconds);
                        }
                    });
        }

        /// <summary>
        /// تنفيذ عملية إرسال البريد مع إعادة المحاولة
        /// </summary>
        public static async Task<bool> ExecuteWithRetryAsync(
            Func<Task<bool>> emailAction,
            ILogger logger,
            string emailAddress,
            int retryAttempts = 3,
            int retryDelaySeconds = 2)
        {
            var retryPolicy = CreateRetryPolicy(logger, retryAttempts, retryDelaySeconds);

            return await retryPolicy.ExecuteAsync(async () =>
            {
                logger.LogInformation("Attempting to send email to {EmailAddress}", emailAddress);
                var result = await emailAction();
                
                // Don't throw exception, just return the result
                // The retry policy will handle false results
                return result;
            });
        }

        /// <summary>
        /// تنفيذ عملية إرسال البريد مع إعدادات مخصصة
        /// </summary>
        public static async Task<bool> ExecuteWithCustomSettingsAsync(
            Func<Task<bool>> emailAction,
            ILogger logger,
            string emailAddress,
            int maxRetries,
            int baseDelaySeconds,
            bool enableExponentialBackoff = true)
        {
            var retryPolicy = Policy<bool>
                .Handle<Exception>()
                .OrResult(result => result == false) // Retry on false result as well
                .WaitAndRetryAsync(
                    retryCount: maxRetries,
                    sleepDurationProvider: retryAttempt => 
                    {
                        if (enableExponentialBackoff)
                        {
                            return TimeSpan.FromSeconds(baseDelaySeconds * Math.Pow(2, retryAttempt - 1));
                        }
                        else
                        {
                            return TimeSpan.FromSeconds(baseDelaySeconds);
                        }
                    },
                    onRetry: (outcome, timeSpan, retryCount, context) =>
                    {
                        if (outcome.Exception != null)
                        {
                            logger.LogWarning(
                                "Email sending attempt {RetryCount}/{MaxRetries} failed. Retrying in {Delay}ms. Error: {Error}",
                                retryCount,
                                maxRetries,
                                timeSpan.TotalMilliseconds,
                                outcome.Exception.ToString());
                        }
                        else
                        {
                            logger.LogWarning(
                                "Email sending attempt {RetryCount}/{MaxRetries} returned false. Retrying in {Delay}ms.",
                                retryCount,
                                maxRetries,
                                timeSpan.TotalMilliseconds);
                        }
                    });

            return await retryPolicy.ExecuteAsync(async () =>
            {
                logger.LogInformation("Attempting to send email to {EmailAddress}", emailAddress);
                
                var result = await emailAction();
                
                // Don't throw exception, just return the result
                // The retry policy will handle false results
                return result;
            });
        }
    }
}
