using csharp_mfca.API.Constants;
using System.Net;

namespace csharp_mfca.API.Utils;

public class BaseResponse<T>
{
    public string RefNo { get; set; }
    public string Message { get; set; }
    public bool IsSuccess { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public T? Data { get; set; }

    public static BaseResponse<T> Success(string refNo, string message = MessageConstant.Success, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new BaseResponse<T>
        {
            Data = default,
            IsSuccess = true,
            Message = message,
            StatusCode = statusCode,
            RefNo = refNo
        };
    }

    public static BaseResponse<T> Success(string refNo, T data, string message = MessageConstant.Success, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new BaseResponse<T>
        {
            Data = data,
            IsSuccess = true,
            Message = message,
            StatusCode = statusCode,
            RefNo = refNo
        };
    }

    public static BaseResponse<T> Fail(string refNo, string message = MessageConstant.Failed, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new BaseResponse<T>
        {
            Data = default,
            IsSuccess = false,
            Message = message,
            StatusCode = statusCode,
            RefNo = refNo
        };
    }

    public static BaseResponse<T> Fail(string refNo, Exception ex, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new BaseResponse<T>
        {
            Data = default,
            IsSuccess = false,
            Message = ex.ToString(),
            StatusCode = statusCode,
            RefNo = refNo
        };
    }
}
