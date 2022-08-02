namespace PodoMicroServices.Common
{
    public class BaseDataResponse<T> : BaseResponse
    {
        public T? Data { get; set; }

        public BaseDataResponse()
        {

        }

        public BaseDataResponse(string message) : base(message)
        {
            Data = default(T);
        }

        public BaseDataResponse(T? data)
        {
            Data = data;
        }
    }
}
