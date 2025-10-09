using System.Net;

namespace ShopPC.Exceptions
{
    public class ErrorCode
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public HttpStatusCode Status { get; set; }

        public ErrorCode(int code, string message, HttpStatusCode status)
        {
            Code = code;
            Message = message;
            Status = status;
        }   

        public static readonly ErrorCode INTERNAL_SERVER_ERROR
            = new ErrorCode(1000, "Uncategorized Exception", HttpStatusCode.InternalServerError);
    }
}
