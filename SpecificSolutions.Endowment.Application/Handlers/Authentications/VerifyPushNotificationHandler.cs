using MediatR;
using SpecificSolutions.Endowment.Application.Models.DTOs;
using SpecificSolutions.Endowment.Application.Services;

namespace SpecificSolutions.Endowment.Application.Handlers.Authentications
{
    public class VerifyPushNotificationCommand : IRequest<EndowmentResponse<VerificationCodeResponse>>
    {
        public string Subscription { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
    }

    public class VerifyPushNotificationHandler : IRequestHandler<VerifyPushNotificationCommand, EndowmentResponse<VerificationCodeResponse>>
    {
        private readonly VerificationCodeService _verificationCodeService;

        public VerifyPushNotificationHandler(VerificationCodeService verificationCodeService)
        {
            _verificationCodeService = verificationCodeService;
        }

        public async Task<EndowmentResponse<VerificationCodeResponse>> Handle(VerifyPushNotificationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // التحقق من صحة الرمز
                var isValid = await _verificationCodeService.VerifyCodeAsync(
                    request.Subscription,
                    request.Code,
                    request.Purpose);

                if (isValid)
                {
                    var purposeText = request.Purpose switch
                    {
                        "EmailConfirmation" => "تم تأكيد البريد الإلكتروني بنجاح عبر الإشعارات الفورية",
                        "PasswordReset" => "تم التحقق من الرمز بنجاح. يمكنك الآن إعادة تعيين كلمة المرور",
                        "TwoFactorAuth" => "تم التحقق من الرمز بنجاح عبر الإشعارات الفورية",
                        _ => "تم التحقق من الرمز بنجاح عبر الإشعارات الفورية"
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
                        "رمز التحقق غير صحيح أو منتهي الصلاحية. يرجى المحاولة مرة أخرى.");
                }
            }
            catch (Exception ex)
            {
                return Response.FailureResponse<VerificationCodeResponse>("",
                    "حدث خطأ أثناء التحقق من الرمز.");
            }
        }
    }
}
