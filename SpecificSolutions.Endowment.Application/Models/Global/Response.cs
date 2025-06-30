using SpecificSolutions.Endowment.Core.Resources;

namespace SpecificSolutions.Endowment.Application.Models.Global
{
    public class Response
    {
        public static EndowmentResponse SuccessLogin()
            => SuccessResponse(ResponseState.Valid, Messages.LoggedinSuccess);

        public static EndowmentResponse<TValue> SuccessLogin<TValue>(TValue? tValue)
            => Responsee(tValue, ResponseState.Valid, Messages.LoggedinSuccess);

        public static EndowmentResponse FailureLogin()
            => FailureResponse(errorMessage: Messages.LoggedinFailed);

        public static EndowmentResponse Added()
            => SuccessResponse(ResponseState.Valid, Messages.Added);

        public static EndowmentResponse Updated()
            => SuccessResponse(ResponseState.Valid, Messages.Updated);

        public static EndowmentResponse Activated()
            => SuccessResponse(ResponseState.Valid, Messages.Activated);

        public static EndowmentResponse Deactivated()
            => SuccessResponse(ResponseState.Valid, Messages.Deactivated);

        public static EndowmentResponse NoChanges()
            => SuccessResponse(ResponseState.Valid, Messages.NoChanges);

        public static EndowmentResponse Deleted()
            => SuccessResponse(ResponseState.Valid, Messages.Deleted);

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
                "Messages.Success"
                );
        }

        public static EndowmentResponse<TValue> GetResponse<TValue>(TValue? tValue)
        {
            return Responsee(ResponseState.Valid,
                tValue,
                "Messages.Success"
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