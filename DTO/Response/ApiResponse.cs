namespace ShopPC.DTO.Response
{
    public class ApiResponse<T>
    {
        public int Code { get; set; } = 1000;
        public string Message { get; set; } = "Success";
        public T? Result { get; set; }

        public ApiResponse() { }

        public ApiResponse(T result, string message = "Success", int code = 1000)
        {
            Result = result;
            Message = message;
            Code = code;
        }
    }
}
