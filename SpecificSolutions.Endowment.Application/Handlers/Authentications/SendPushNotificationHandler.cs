using MediatR;
using SpecificSolutions.Endowment.Application.Models.DTOs;
using SpecificSolutions.Endowment.Application.Services;
using Microsoft.AspNetCore.Http;

namespace SpecificSolutions.Endowment.Application.Handlers.Authentications
{
    public class SendPushNotificationCommand : IRequest<EndowmentResponse<VerificationCodeResponse>>
    {
        public string Subscription { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
    }

    public class SendPushNotificationHandler : IRequestHandler<SendPushNotificationCommand, EndowmentResponse<VerificationCodeResponse>>
    {
        private readonly VerificationCodeService _verificationCodeService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SendPushNotificationHandler(
            VerificationCodeService verificationCodeService,
            IHttpContextAccessor httpContextAccessor)
        {
            _verificationCodeService = verificationCodeService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<EndowmentResponse<VerificationCodeResponse>> Handle(SendPushNotificationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // الحصول على IP Address و User Agent إذا لم يتم تمريرهما
                var ipAddress = request.IpAddress ?? GetClientIpAddress();
                var userAgent = request.UserAgent ?? GetUserAgent();

                // إرسال رمز التحقق عبر Push Notification
                var success = await _verificationCodeService.SendVerificationCodeViaPushNotificationAsync(
                    request.Subscription,
                    request.Purpose,
                    request.UserId,
                    ipAddress,
                    userAgent);

                if (success)
                {
                    var purposeText = request.Purpose switch
                    {
                        "EmailConfirmation" => "تم إرسال رمز التحقق عبر الإشعارات الفورية",
                        "PasswordReset" => "تم إرسال رمز إعادة تعيين كلمة المرور عبر الإشعارات الفورية",
                        "TwoFactorAuth" => "تم إرسال رمز المصادقة الثنائية عبر الإشعارات الفورية",
                        _ => "تم إرسال رمز التحقق عبر الإشعارات الفورية"
                    };

                    return Response.GetResponse(
                        new VerificationCodeResponse
                        {
                            IsSuccess = true,
                            Message = purposeText,
                            Purpose = request.Purpose
                        },
                        purposeText);
                }
                else
                {
                    return Response.FailureResponse<VerificationCodeResponse>("",
                        "فشل في إرسال رمز التحقق عبر الإشعارات الفورية. يرجى المحاولة مرة أخرى.");
                }
            }
            catch (Exception ex)
            {
                return Response.FailureResponse<VerificationCodeResponse>("",
                    "حدث خطأ أثناء إرسال رمز التحقق عبر الإشعارات الفورية.");
            }
        }

        private string GetClientIpAddress()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) return string.Empty;

            var forwardedHeader = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedHeader))
            {
                return forwardedHeader.Split(',')[0].Trim();
            }

            return httpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
        }

        private string GetUserAgent()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            return httpContext?.Request.Headers["User-Agent"].FirstOrDefault() ?? string.Empty;
        }
    }
}
