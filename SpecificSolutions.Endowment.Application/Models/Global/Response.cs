using System.Text.Json.Serialization;

namespace SpecificSolutions.Endowment.Application.Models.Global
{
    public class Response
    {
        public static EndowmentResponse SuccessLogin()
            => SuccessResponse(ResponseState.Valid, "تم تسجيل الدخول بنجاح");

        public static EndowmentResponse<TValue> SuccessLogin<TValue>(TValue? tValue)
            => Responsee(tValue, ResponseState.Valid, "تم تسجيل الدخول بنجاح");

        public static EndowmentResponse FailureLogin()
            => FailureResponse(errorMessage: "فشل في تسجيل الدخول");

        public static EndowmentResponse Added()
            => SuccessResponse(ResponseState.Valid, "تم الإضافة بنجاح");

        public static EndowmentResponse Updated()
            => SuccessResponse(ResponseState.Valid, "تم التحديث بنجاح");

        public static EndowmentResponse Activated()
            => SuccessResponse(ResponseState.Valid, "تم التفعيل بنجاح");

        public static EndowmentResponse Deactivated()
            => SuccessResponse(ResponseState.Valid, "تم إلغاء التفعيل بنجاح");

        public static EndowmentResponse NoChanges()
            => SuccessResponse(ResponseState.Valid, "لا توجد تغييرات");

        public static EndowmentResponse Deleted()
            => SuccessResponse(ResponseState.Valid, "تم الحذف بنجاح");

        public static EndowmentResponse FailureResponse(string propertyName = "", string errorMessage = "", params object[] args)
        {
            var localizedMessage = args.Length > 0
                ? string.Format(errorMessage, args)
                : errorMessage;

            return Responsee(ResponseState.BadRequest,
                new Error[] { new(propertyName, localizedMessage) },
                string.Empty,
                false);
        }

        public static EndowmentResponse SuccessResponse(ResponseState state, string messages = "", params object[] args)
        {
            var localizedMessage = args.Length > 0
                ? string.Format(messages, args)
                : messages;

            return Responsee(state,
                null,
                localizedMessage,
                true);
        }

        private static EndowmentResponse Responsee(ResponseState state, Error[]? errors, string message, bool isSuccess)
        {
            return new EndowmentResponse(state, message, errors ?? Array.Empty<Error>());
        }

        public static EndowmentResponse<TValue> FailureResponse<TValue>(string propertyName = "", string errorMessage = "", params object[] args)
        {
            var localizedMessage = args.Length > 0
                         ? string.Format(errorMessage, args)
                         : errorMessage;

            return Responsee<TValue>(
                default,
                string.Empty,
                new Error[] { new(propertyName, localizedMessage) }
                );

        }

        public static EndowmentResponse<TValue> FilterResponse<TValue>(TValue? tValue)
        {
            return Responsee(ResponseState.Valid,
                tValue,
                "تمت العملية بنجاح"
                );
        }

        public static EndowmentResponse<TValue> GetResponse<TValue>(TValue? tValue)
        {
            return Responsee(ResponseState.Valid,
                tValue,
                "تمت العملية بنجاح"
                );
        }

        public static EndowmentResponse<TValue> Responsee<TValue>(TValue? tValue, string errorMessage = "", params Error[]? errors)
        {
            return new EndowmentResponse<TValue>(tValue, errors, errorMessage);
        }

        public static EndowmentResponse<TValue> Responsee<TValue>(ResponseState state, TValue? tValue, string errorMessage = "", params Error[]? errors)
        {
            return new EndowmentResponse<TValue>(tValue, state, errors, errorMessage);
        }

        public static EndowmentResponse<TValue> Responsee<TValue>(TValue? tValue, ResponseState state, string message = "")
        {
            return new EndowmentResponse<TValue>(tValue, state, message);
        }
    }
}