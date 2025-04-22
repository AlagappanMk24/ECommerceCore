namespace ECommerceCore.Application.Common.Results
{
    public class OperationResult<T>
    {
        public bool IsSuccess { get; }
        public T Data { get; }
        public string ErrorMessage { get; }

        private OperationResult(bool isSuccess, T data, string errorMessage)
        {
            IsSuccess = isSuccess;
            Data = data;
            ErrorMessage = errorMessage;
        }
        public static OperationResult<T> SuccessResult(T data)
            => new OperationResult<T>(true, data, null);

        public static OperationResult<T> FailureResult(string errorMessage)
            => new OperationResult<T>(false, default, errorMessage);
    }
}
