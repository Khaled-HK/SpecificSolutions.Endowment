using MediatR;
using SpecificSolutions.Endowment.Application.Models.DTOs;
using SpecificSolutions.Endowment.Application.Services;
using Microsoft.AspNetCore.Http;

namespace SpecificSolutions.Endowment.Application.Handlers.Authentications
{
    public class SendVerificationCodeCommand : IRequest<EndowmentResponse<VerificationCodeResponse>>
    {
        public string Email { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
    }

    public class SendVerificationCodeHandler : IRequestHandler<SendVerificationCodeCommand, EndowmentResponse<VerificationCodeResponse>>
    {
        private readonly VerificationCodeService _verificationCodeService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SendVerificationCodeHandler(
            VerificationCodeService verificationCodeService,
            IHttpContextAccessor httpContextAccessor)
        {
            _verificationCodeService = verificationCodeService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<EndowmentResponse<VerificationCodeResponse>> Handle(SendVerificationCodeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // الحصول على IP Address و User Agent إذا لم يتم تمريرهما
                var ipAddress = request.IpAddress ?? GetClientIpAddress();
                var userAgent = request.UserAgent ?? GetUserAgent();

                // إرسال رمز التحقق
                var success = await _verificationCodeService.SendVerificationCodeAsync(
                    request.Email,
                    request.Purpose,
                    request.UserId,
                    ipAddress,
                    userAgent);

                if (success)
                {
                    return Response.GetResponse(
                        new VerificationCodeResponse
                        {
                            IsSuccess = true,
                            Message = "تم إرسال رمز التحقق بنجاح. تحقق من بريدك الإلكتروني.",
                            Email = request.Email,
                            Purpose = request.Purpose
                        });
                }
                else
                {
                    return Response.FailureResponse("", "فشل في إرسال رمز التحقق. يرجى المحاولة مرة أخرى لاحقاً.");
                }
            }
            catch (Exception ex)
            {
                return Response.FailureResponse("", $"خطأ في إرسال رمز التحقق: {ex.Message}");
            }
        }

        private string GetClientIpAddress()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) return "Unknown";

            // الحصول على IP Address الحقيقي
            var forwardedHeader = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedHeader))
            {
                return forwardedHeader.Split(',')[0].Trim();
            }

            var remoteIpAddress = httpContext.Connection.RemoteIpAddress;
            return remoteIpAddress?.ToString() ?? "Unknown";
        }

        private string GetUserAgent()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            return httpContext?.Request.Headers["User-Agent"].FirstOrDefault() ?? "Unknown";
        }
    }
}
