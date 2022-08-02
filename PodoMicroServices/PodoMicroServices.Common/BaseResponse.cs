namespace PodoMicroServices.Common
{
    public class BaseResponse
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;

        public BaseResponse()
        {

        }

        public BaseResponse(string message)
        {
            Message = message;
            Success = false;
        }
    }
}
