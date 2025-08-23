using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Models.Global;
using System.Collections.Concurrent;

namespace SpecificSolutions.Endowment.Infrastructure.Services
{
    /// <summary>
    /// خدمة خلفية لإرسال البريد الإلكتروني - النسخة المجانية المحسنة
    /// </summary>
    public class EmailBackgroundService : BackgroundService
    {
        private readonly ILogger<EmailBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly EmailSettings _emailSettings;
        private readonly ConcurrentQueue<EmailRequest> _emailQueue;
        private readonly SemaphoreSlim _semaphore;
        private readonly ConcurrentDictionary<string, DateTime> _emailRateLimit;

        public EmailBackgroundService(
            ILogger<EmailBackgroundService> logger,
            IServiceProvider serviceProvider,
            IOptions<EmailSettings> emailSettings)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _emailSettings = emailSettings.Value;
            _emailQueue = new ConcurrentQueue<EmailRequest>();
            _semaphore = new SemaphoreSlim(1, 1);
            _emailRateLimit = new ConcurrentDictionary<string, DateTime>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Email Background Service started with settings: Queue={EnableQueue}, Interval={Interval}s", 
                _emailSettings.EnableEmailQueue, _emailSettings.QueueProcessingIntervalSeconds);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (_emailSettings.EnableEmailQueue)
                    {
                        await ProcessEmailQueue(stoppingToken);
                    }
                    
                    await Task.Delay(TimeSpan.FromSeconds(_emailSettings.QueueProcessingIntervalSeconds), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Email Background Service is stopping");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while processing email queue");
                    await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken); // Wait longer on error
                }
            }

            _logger.LogInformation("Email Background Service stopped");
        }

        /// <summary>
        /// إضافة بريد إلكتروني إلى قائمة الانتظار مع Rate Limiting
        /// </summary>
        public async Task EnqueueEmailAsync(EmailRequest emailRequest)
        {
            if (!_emailSettings.EnableEmailQueue)
            {
                _logger.LogWarning("Email queue is disabled. Processing email immediately.");
                await ProcessEmailImmediatelyAsync(emailRequest);
                return;
            }

            // التحقق من Rate Limiting
            if (!CheckRateLimit(emailRequest.To))
            {
                _logger.LogWarning("Rate limit exceeded for email: {Email}. Email will be queued for later processing.", emailRequest.To);
            }

            await _semaphore.WaitAsync();
            try
            {
                _emailQueue.Enqueue(emailRequest);
                _logger.LogInformation("Email queued for {Email}. Queue size: {QueueSize}", 
                    emailRequest.To, _emailQueue.Count);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <summary>
        /// معالجة قائمة انتظار البريد الإلكتروني
        /// </summary>
        private async Task ProcessEmailQueue(CancellationToken stoppingToken)
        {
            await _semaphore.WaitAsync(stoppingToken);
            try
            {
                var emailsToProcess = new List<EmailRequest>();
                
                // Dequeue all emails from the queue
                while (_emailQueue.TryDequeue(out var emailRequest))
                {
                    emailsToProcess.Add(emailRequest);
                }

                if (emailsToProcess.Any())
                {
                    _logger.LogInformation("Processing {Count} emails from queue", emailsToProcess.Count);
                    
                    // Process emails with rate limiting
                    foreach (var email in emailsToProcess)
                    {
                        if (CheckRateLimit(email.To))
                        {
                            await ProcessEmailAsync(email, stoppingToken);
                        }
                        else
                        {
                            // Re-queue email for later processing
                            _emailQueue.Enqueue(email);
                            _logger.LogInformation("Email re-queued for {Email} due to rate limiting", email.To);
                        }
                    }
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <summary>
        /// معالجة بريد إلكتروني فوري (بدون قائمة انتظار)
        /// </summary>
        private async Task ProcessEmailImmediatelyAsync(EmailRequest emailRequest)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                _logger.LogInformation("Processing email immediately for {Email}", emailRequest.To);

                bool result;
                switch (emailRequest.Type)
                {
                    case EmailType.Confirmation:
                        result = await emailService.SendEmailConfirmationAsync(emailRequest.To, emailRequest.Link);
                        break;
                    case EmailType.PasswordReset:
                        result = await emailService.SendPasswordResetAsync(emailRequest.To, emailRequest.Link);
                        break;
                    case EmailType.General:
                        result = await emailService.SendEmailAsync(emailRequest.To, emailRequest.Subject, emailRequest.Body);
                        break;
                    default:
                        _logger.LogWarning("Unknown email type: {EmailType}", emailRequest.Type);
                        return;
                }

                if (result)
                {
                    _logger.LogInformation("Email processed successfully for {Email}", emailRequest.To);
                }
                else
                {
                    _logger.LogError("Failed to process email for {Email}", emailRequest.To);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing email immediately for {Email}", emailRequest.To);
            }
        }

        /// <summary>
        /// معالجة بريد إلكتروني واحد
        /// </summary>
        private async Task ProcessEmailAsync(EmailRequest emailRequest, CancellationToken stoppingToken)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                _logger.LogInformation("Sending email to {Email}", emailRequest.To);

                bool result;
                switch (emailRequest.Type)
                {
                    case EmailType.Confirmation:
                        result = await emailService.SendEmailConfirmationAsync(emailRequest.To, emailRequest.Link);
                        break;
                    case EmailType.PasswordReset:
                        result = await emailService.SendPasswordResetAsync(emailRequest.To, emailRequest.Link);
                        break;
                    case EmailType.General:
                        result = await emailService.SendEmailAsync(emailRequest.To, emailRequest.Subject, emailRequest.Body);
                        break;
                    default:
                        _logger.LogWarning("Unknown email type: {EmailType}", emailRequest.Type);
                        return;
                }

                if (result)
                {
                    _logger.LogInformation("Email sent successfully to {Email}", emailRequest.To);
                }
                else
                {
                    _logger.LogError("Failed to send email to {Email}", emailRequest.To);
                    // Re-queue failed emails (with retry limit)
                    if (emailRequest.RetryCount < _emailSettings.RetryAttempts)
                    {
                        emailRequest.RetryCount++;
                        _emailQueue.Enqueue(emailRequest);
                        _logger.LogInformation("Re-queued email for {Email} (retry {RetryCount}/{MaxRetries})", 
                            emailRequest.To, emailRequest.RetryCount, _emailSettings.RetryAttempts);
                    }
                    else
                    {
                        _logger.LogError("Email failed permanently for {Email} after {RetryCount} attempts", 
                            emailRequest.To, emailRequest.RetryCount);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing email for {Email}", emailRequest.To);
                
                // Re-queue on error (with retry limit)
                if (emailRequest.RetryCount < _emailSettings.RetryAttempts)
                {
                    emailRequest.RetryCount++;
                    _emailQueue.Enqueue(emailRequest);
                    _logger.LogInformation("Re-queued email for {Email} after error (retry {RetryCount}/{MaxRetries})", 
                        emailRequest.To, emailRequest.RetryCount, _emailSettings.RetryAttempts);
                }
                else
                {
                    _logger.LogError("Email failed permanently for {Email} after {RetryCount} attempts due to errors", 
                        emailRequest.To, emailRequest.RetryCount);
                }
            }
        }

        /// <summary>
        /// التحقق من Rate Limiting
        /// </summary>
        private bool CheckRateLimit(string email)
        {
            var now = DateTime.UtcNow;
            var lastEmailTime = _emailRateLimit.GetOrAdd(email, now);

            if ((now - lastEmailTime).TotalMinutes < 1.0 / _emailSettings.EmailRateLimit)
            {
                return false;
            }

            _emailRateLimit[email] = now;
            return true;
        }

        /// <summary>
        /// الحصول على إحصائيات قائمة الانتظار
        /// </summary>
        public Dictionary<string, object> GetQueueStatistics()
        {
            return new Dictionary<string, object>
            {
                ["QueueSize"] = _emailQueue.Count,
                ["EnableEmailQueue"] = _emailSettings.EnableEmailQueue,
                ["QueueProcessingIntervalSeconds"] = _emailSettings.QueueProcessingIntervalSeconds,
                ["EmailRateLimit"] = _emailSettings.EmailRateLimit,
                ["RetryAttempts"] = _emailSettings.RetryAttempts
            };
        }

        public override void Dispose()
        {
            _semaphore?.Dispose();
            base.Dispose();
        }
    }

    /// <summary>
    /// نموذج طلب البريد الإلكتروني - النسخة المحسنة
    /// </summary>
    public class EmailRequest
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public EmailType Type { get; set; }
        public int RetryCount { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? UserId { get; set; }
        public string? SessionId { get; set; }
    }

    /// <summary>
    /// أنواع البريد الإلكتروني
    /// </summary>
    public enum EmailType
    {
        General,
        Confirmation,
        PasswordReset,
        Notification,
        Newsletter
    }
}
