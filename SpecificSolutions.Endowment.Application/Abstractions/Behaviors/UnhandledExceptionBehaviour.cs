using MediatR;
using Microsoft.Extensions.Logging;
using SpecificSolutions.Endowment.Application.Abstractions.Exceptions;

namespace SpecificSolutions.Endowment.Application.Abstractions.Behaviors
{
    /// <summary>
    /// نمط خالد: معالجة الأخطاء الموحدة لجميع الـ Commands والـ Queries
    /// </summary>
    public class UnhandledExceptionBehaviour<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
        where TResponse : notnull
    {
        private readonly ILogger<UnhandledExceptionBehaviour<TRequest, TResponse>> _logger;

        public UnhandledExceptionBehaviour(ILogger<UnhandledExceptionBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (ValidationException ex)
            {
                var requestName = typeof(TRequest).Name;
                _logger.LogWarning(ex, "Validation Exception for Request {Name}", requestName);

                // إعادة رمي الاستثناء ليتم التعامل معه في GlobalExceptionHandler
                throw;
            }
            catch (UnauthorizedAccessException ex)
            {
                var requestName = typeof(TRequest).Name;
                _logger.LogWarning(ex, "Unauthorized Access Exception for Request {Name}", requestName);

                // نمط خالد: رمي استثناء مخصص مع رسالة عربية
                //throw new UnauthorizedAccessException("لم يتم الموافقة على حسابك بعد. يرجى انتظار موافقة المسؤول.");
                throw;
            }
            catch (NotFoundException ex)
            {
                var requestName = typeof(TRequest).Name;
                _logger.LogWarning(ex, "Not Found Exception for Request {Name}", requestName);

                throw;
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;
                _logger.LogError(ex, "Unhandled Exception for Request {Name} {@Request}", requestName, request);

                // نمط خالد: رمي استثناء عام مع رسالة عربية
                throw new Exception("حدث خطأ أثناء تسجيل الدخول. يرجى المحاولة مرة أخرى.");
            }
        }
    }
}
