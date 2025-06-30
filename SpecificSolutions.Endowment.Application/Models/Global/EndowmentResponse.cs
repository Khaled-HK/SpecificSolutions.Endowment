namespace SpecificSolutions.Endowment.Application.Models.Global
{
    public class EndowmentResponse
    {
        protected EndowmentResponse() { }

        public EndowmentResponse(string? message, Error[] errors)
        {
            Message = message;
            Errors = errors;
            State = ResponseState.Valid;
        }

        public EndowmentResponse(ResponseState state, string? message)
        {
            Message = message;
            State = state;
        }

        public EndowmentResponse(ResponseState state, string? message, Error[]? errors)
        {
            Message = message;
            Errors = errors;
            State = state;
        }

        public Error[]? Errors { get; set; } = Array.Empty<Error>();

        private string _message;
        public string Message
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_message))
                    return _message;

                var errors = string.Join("\n", Errors?.SelectMany(e => e.ErrorMessage));

                return string.IsNullOrWhiteSpace(errors) ? TranslateState(_state) : errors;
            }
            private set => _message = value;
        }

        public bool IsSuccess => State == ResponseState.Valid;

        private ResponseState _state;

        public ResponseState State
        {
            get => _state == ResponseState.Valid && Errors?.Length > 0
                    ? ResponseState.BadRequest
                    : _state;

            private set => _state = value;
        }

        private string TranslateState(ResponseState state)
        {
            switch (state)
            {
                default:
                    return "";
            }
        }
    }

    public class EndowmentResponse<TData> : EndowmentResponse
    {
        public TData? Data { get; set; }

        public EndowmentResponse(TData? data) : base(null, null)
        {
            Data = data;
        }

        public EndowmentResponse(TData? data, ResponseState state) : base(state, null, null)
        {
            Data = data;
        }

        public EndowmentResponse(TData? data, ResponseState state, string? message) : base(state, message, null)
        {
            Data = data;
        }

        public EndowmentResponse(Error[]? errors) : base(null, errors) { }

        public EndowmentResponse(TData? data, ResponseState state, Error[]? errors, string? message)
            : base(state, message, errors)
        {
            Data = data;
        }

        public EndowmentResponse(TData? data, Error[]? errors, string? message)
            : base(message, errors)
        {
            Data = data;
        }

        public EndowmentResponse(ResponseState state, string? message)
            : base(state, message)
        {
        }
    }

    public enum ResponseState
    {

        Valid = 1,

        BadRequest = 400,

        NotFound = 404,

        Forbidden = 403,

        Unauthorized = 401,

        Unavailable = 503,

        Unacceptable = 406
    }


    //public static EndowmentResponse<TData> ValidationFailure(Error[]? error) =>
    //    new(error);
}