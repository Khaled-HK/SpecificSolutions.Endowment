using MediatR;
using SpecificSolutions.Endowment.Application.Models.DTOs;
using SpecificSolutions.Endowment.Application.Services;

namespace SpecificSolutions.Endowment.Application.Handlers.Authentications
{
    public class VerifyCodeCommand : IRequest<EndowmentResponse<VerificationCodeResponse>>
    {
        public string Email { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
    }

    public class VerifyCodeHandler : IRequestHandler<VerifyCodeCommand, EndowmentResponse<VerificationCodeResponse>>
    {
        private readonly VerificationCodeService _verificationCodeService;

        public VerifyCodeHandler(VerificationCodeService verificationCodeService)
        {
            _verificationCodeService = verificationCodeService;
        }

        public async Task<EndowmentResponse<VerificationCodeResponse>> Handle(VerifyCodeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // التحقق من صحة الرمز
                var isValid = await _verificationCodeService.VerifyCodeAsync(
                    request.Email,
                    request.Code,
                    request.Purpose);

                if (isValid)
                {
                    var purposeText = request.Purpose switch
                    {
                        "EmailConfirmation" => "تم تأكيد البريد الإلكتروني بنجاح",
                        "PasswordReset" => "تم التحقق من الرمز بنجاح. يمكنك الآن إعادة تعيين كلمة المرور",
                        "TwoFactorAuth" => "تم التحقق من الرمز بنجاح",
                        _ => "تم التحقق من الرمز بنجاح"
                    };

                    return Response.GetResponse(
                        new VerificationCodeResponse
                        {
                            IsSuccess = true,
                            Message = purposeText,
                            Email = request.Email,
                            Purpose = request.Purpose
                        });
                }
                else
                {
                    return Response.FailureResponse("", "رمز التحقق غير صحيح أو منتهي الصلاحية. يرجى المحاولة مرة أخرى.");
                }
            }
            catch (Exception ex)
            {
                return Response.FailureResponse("", $"خطأ في التحقق من الرمز: {ex.Message}");
            }
        }
    }
}
