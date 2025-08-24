using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.ResendEmailConfirmation
{
    public class ResendEmailConfirmationHandler : ICommandHandler<ResendEmailConfirmationCommand>
    {
        private readonly IAuthenticator _authenticator;

        public ResendEmailConfirmationHandler(IAuthenticator authenticator)
        {
            _authenticator = authenticator;
        }

        /// <summary>
        /// نمط خالد: Handler نظيف بدون try-catch - الأخطاء تُعالج في Pipeline Behavior
        /// </summary>
        public async Task<EndowmentResponse> Handle(ResendEmailConfirmationCommand command, CancellationToken cancellationToken)
        {
            var result = await _authenticator.ResendEmailConfirmationAsync(command.Email);
            
            if (!result)
            {
                return Response.FailureResponse("ResendEmail", "فشل في إعادة إرسال بريد التأكيد");
            }

            return Response.SuccessResponse(ResponseState.Valid, "تم إرسال بريد التأكيد بنجاح. تحقق من صندوق الوارد الخاص بك.");
        }
    }
}
