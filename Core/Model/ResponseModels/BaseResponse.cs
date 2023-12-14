using Core.Model.ErrorModels;
using System.Net;
using System.Text.Json.Serialization;

namespace Core.Model.ResponseModels;

public class BaseResponse : BaseResponse<ErrorModel>
{
    public BaseResponse(HttpStatusCode statusCode, string message) : base(statusCode, message)
    {
    }
    public BaseResponse(HttpStatusCode statusCode, params ErrorModel[] errors) : base(statusCode, errors)
    {
    }

    public BaseResponse(HttpStatusCode statusCode, IEnumerable<ErrorModel> errors) : base(statusCode, errors)
    {
    }
    public BaseResponse(HttpStatusCode statusCode, string message, params ErrorModel[] errors) : base(statusCode, message, errors)
    {
    }
    public BaseResponse(HttpStatusCode statusCode, string message, IEnumerable<ErrorModel> errors) : base(statusCode, message, errors)
    {
    }
    public static BaseResponse Success() => new BaseResponse(HttpStatusCode.OK); 
    public static BaseResponse Fail(params ErrorModel[] errors) => new BaseResponse(HttpStatusCode.BadRequest, errors);
    public static BaseResponse Fail(IEnumerable<ErrorModel> errors) => new BaseResponse(HttpStatusCode.BadRequest, errors);
    public static BaseResponse Fail(string message) => new BaseResponse(HttpStatusCode.BadRequest, message);
    public static BaseResponse Fail(string message, params ErrorModel[] errors) => new BaseResponse(HttpStatusCode.BadRequest, message, errors);
    public static BaseResponse Fail(string message, IEnumerable<ErrorModel> errors) => new BaseResponse(HttpStatusCode.BadRequest, message, errors);
    public static BaseResponse NotFound(string message) => new BaseResponse(HttpStatusCode.NotFound, message);
    public static BaseResponse NotFound() => new BaseResponse(HttpStatusCode.NotFound, "Entity not found");
    public static BaseResponse Unauthorized(string message) => new BaseResponse(HttpStatusCode.Unauthorized, message);
}

public class BaseResponse<TError>
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ResponseError<TError> Errors { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public HttpStatusCode StatusCode { get; }
    [System.Text.Json.Serialization.JsonIgnore]
    public bool IsSuccess { get; set; }

    public BaseResponse(HttpStatusCode statusCode)
    {
        IsSuccess = statusCode == HttpStatusCode.OK;
        StatusCode = statusCode;
    }
    /// <summary>
    /// Creates a BaseResponse .
    /// </summary>
    /// <param>TModel value.</param>
    /// <param name="statusCode"></param>
    /// <param name="errors">TMessage messsages.</param>
    /// <returns>BaseResponse</returns>
    public BaseResponse(HttpStatusCode statusCode, params TError[] errors) : this(statusCode)
    {
        if (!IsSuccess && errors != null && errors.Length > 0) Errors = new ResponseError<TError>(errors);
    }


    public BaseResponse(HttpStatusCode statusCode, IEnumerable<TError> errors) : this(statusCode)
    {
        Errors = new ResponseError<TError>(errors);
    }
    public BaseResponse(HttpStatusCode statusCode, string message) : this(statusCode)
    {
        Errors = new ResponseError<TError>(message);
    }
    public BaseResponse(HttpStatusCode statusCode, string message, IEnumerable<TError> errors) : this(statusCode)
    {
        Errors = new ResponseError<TError>(message, errors);
    }
    public BaseResponse(HttpStatusCode statusCode, string message, params TError[] errors) : this(statusCode)
    {
        Errors = new ResponseError<TError>(message, errors);
    }
}
