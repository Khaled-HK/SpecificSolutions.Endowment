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

        /// <summary>
        /// إرسال رمز تحقق عبر Push Notification
        /// </summary>
        [HttpPost("send-push")]
        public async Task<ActionResult<EndowmentResponse<VerificationCodeResponse>>> SendPushNotification([FromBody] SendPushNotificationRequest request)
        {
            var command = new SendPushNotificationCommand
            {
                Subscription = request.Subscription,
                Purpose = request.Purpose
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// التحقق من رمز Push Notification
        /// </summary>
        [HttpPost("verify-push")]
        public async Task<ActionResult<EndowmentResponse<VerificationCodeResponse>>> VerifyPushNotification([FromBody] VerifyPushNotificationRequest request)
        {
            var command = new VerifyPushNotificationCommand
            {
                Subscription = request.Subscription,
                Code = request.Code,
                Purpose = request.Purpose
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// إرسال رمز تحقق عبر SMS
        /// </summary>
        [HttpPost("send-sms")]
        public async Task<ActionResult<EndowmentResponse<VerificationCodeResponse>>> SendSmsVerificationCode([FromBody] SendSmsVerificationCodeRequest request)
        {
            var command = new SendSmsVerificationCodeCommand
            {
                PhoneNumber = request.PhoneNumber,
                Purpose = request.Purpose
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// التحقق من رمز SMS
        /// </summary>
        [HttpPost("verify-sms")]
        public async Task<ActionResult<EndowmentResponse<VerificationCodeResponse>>> VerifySmsCode([FromBody] VerifySmsCodeRequest request)
        {
            var command = new VerifySmsCodeCommand
            {
                PhoneNumber = request.PhoneNumber,
                Code = request.Code,
                Purpose = request.Purpose
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// إرسال رمز تحقق عبر Telegram
        /// </summary>
        [HttpPost("send-telegram")]
        public async Task<ActionResult<EndowmentResponse<VerificationCodeResponse>>> SendTelegramVerificationCode([FromBody] SendTelegramVerificationCodeRequest request)
        {
            var command = new SendTelegramVerificationCodeCommand
            {
                ChatId = request.ChatId,
                Purpose = request.Purpose
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// التحقق من رمز Telegram
        /// </summary>
        [HttpPost("verify-telegram")]
        public async Task<ActionResult<EndowmentResponse<VerificationCodeResponse>>> VerifyTelegramCode([FromBody] VerifyTelegramCodeRequest request)
        {
            var command = new VerifyTelegramCodeCommand
            {
                ChatId = request.ChatId,
                Code = request.Code,
                Purpose = request.Purpose
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
