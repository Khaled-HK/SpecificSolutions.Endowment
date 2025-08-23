using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using System.Collections.Concurrent;

namespace SpecificSolutions.Endowment.Infrastructure.Services
{
    /// <summary>
    /// خدمة خلفية لإرسال البريد الإلكتروني
    /// </summary>
    public class EmailBackgroundService : BackgroundService
    {
        private readonly ILogger<EmailBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentQueue<EmailRequest> _emailQueue;
        private readonly SemaphoreSlim _semaphore;

        public EmailBackgroundService(
            ILogger<EmailBackgroundService> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _emailQueue = new ConcurrentQueue<EmailRequest>();
            _semaphore = new SemaphoreSlim(1, 1);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Email Background Service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessEmailQueue(stoppingToken);
                    await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken); // Process every 30 seconds
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
        /// إضافة بريد إلكتروني إلى قائمة الانتظار
        /// </summary>
        public async Task EnqueueEmailAsync(EmailRequest emailRequest)
        {
            await _semaphore.WaitAsync();
            try
            {
                _emailQueue.Enqueue(emailRequest);
                _logger.LogInformation("Email queued for {Email}", emailRequest.To);
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
                    
                    // Process emails in parallel (with limited concurrency)
                    var tasks = emailsToProcess.Select(email => ProcessEmailAsync(email, stoppingToken));
                    await Task.WhenAll(tasks);
                }
            }
            finally
            {
                _semaphore.Release();
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
                    if (emailRequest.RetryCount < 3)
                    {
                        emailRequest.RetryCount++;
                        _emailQueue.Enqueue(emailRequest);
                        _logger.LogInformation("Re-queued email for {Email} (retry {RetryCount})", emailRequest.To, emailRequest.RetryCount);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing email for {Email}", emailRequest.To);
                
                // Re-queue on error (with retry limit)
                if (emailRequest.RetryCount < 3)
                {
                    emailRequest.RetryCount++;
                    _emailQueue.Enqueue(emailRequest);
                    _logger.LogInformation("Re-queued email for {Email} after error (retry {RetryCount})", emailRequest.To, emailRequest.RetryCount);
                }
            }
        }

        public override void Dispose()
        {
            _semaphore?.Dispose();
            base.Dispose();
        }
    }

    /// <summary>
    /// نموذج طلب البريد الإلكتروني
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
    }

    /// <summary>
    /// أنواع البريد الإلكتروني
    /// </summary>
    public enum EmailType
    {
        General,
        Confirmation,
        PasswordReset
    }
}
