using Microsoft.AspNetCore.Mvc;
using MediatR;
using SpecificSolutions.Endowment.Application.Handlers.Authentications;
using SpecificSolutions.Endowment.Application.Models.DTOs;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VerificationCodeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VerificationCodeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// إرسال رمز التحقق
        /// </summary>
        [HttpPost("send")]
        public async Task<ActionResult<EndowmentResponse<VerificationCodeResponse>>> SendVerificationCode([FromBody] SendVerificationCodeRequest request)
        {
            var command = new SendVerificationCodeCommand
            {
                Email = request.Email,
                Purpose = request.Purpose
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// التحقق من رمز التحقق
        /// </summary>
        [HttpPost("verify")]
        public async Task<ActionResult<EndowmentResponse<VerificationCodeResponse>>> VerifyCode([FromBody] VerifyCodeRequest request)
        {
            var command = new VerifyCodeCommand
            {
                Email = request.Email,
                Code = request.Code,
                Purpose = request.Purpose
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// إعادة إرسال رمز التحقق
        /// </summary>
        [HttpPost("resend")]
        public async Task<ActionResult<EndowmentResponse<VerificationCodeResponse>>> ResendVerificationCode([FromBody] ResendVerificationCodeRequest request)
        {
            var command = new SendVerificationCodeCommand
            {
                Email = request.Email,
                Purpose = request.Purpose
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// تأكيد البريد الإلكتروني بالرمز (للتوافق مع الفرونت إند)
        /// </summary>
        [HttpPost("confirm-email-with-code")]
        public async Task<ActionResult<EndowmentResponse<VerificationCodeResponse>>> ConfirmEmailWithCode([FromBody] VerifyCodeRequest request)
        {
            // تعيين الغرض كـ EmailConfirmation
            request.Purpose = "EmailConfirmation";

            var command = new VerifyCodeCommand
            {
                Email = request.Email,
                Code = request.Code,
                Purpose = request.Purpose
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// إعادة إرسال رمز التحقق (للتوافق مع الفرونت إند)
        /// </summary>
        [HttpPost("resend-verification-code")]
        public async Task<ActionResult<EndowmentResponse<VerificationCodeResponse>>> ResendVerificationCodeForFrontend([FromBody] ResendVerificationCodeRequest request)
        {
            var command = new SendVerificationCodeCommand
            {
                Email = request.Email,
                Purpose = request.Purpose
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
