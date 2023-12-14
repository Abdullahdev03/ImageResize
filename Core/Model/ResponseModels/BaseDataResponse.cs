using Core.Model.ErrorModels;
using System.Net;
using System.Text.Json.Serialization;

namespace Core.Model.ResponseModels;

public class BaseDataResponse<TData> : BaseDataResponse<TData, ErrorModel> where TData : class
{
    public BaseDataResponse(TData data, HttpStatusCode statusCode, string message) : base(data, statusCode, message)
    {
    }
    /// <summary>
    /// Creates a BaseDataResponse .
    /// </summary>
    /// <param name="data">TModel value.</param>
    /// <param name="statusCode"></param>
    /// <param name="errors">String messsages.</param>
    /// <returns>BaseDataResponse</returns>
    public BaseDataResponse(TData data, HttpStatusCode statusCode, params ErrorModel[] errors) : base(data, statusCode, errors)
    {
    }
    public BaseDataResponse(TData data, HttpStatusCode statusCode, IEnumerable<ErrorModel> errors) : base(data, statusCode, errors)
    {
    }
    public BaseDataResponse(TData data, HttpStatusCode statusCode, string message, params ErrorModel[] errors) : base(data, statusCode, message, errors)
    {
    }
    public BaseDataResponse(TData data, HttpStatusCode statusCode, string message, IEnumerable<ErrorModel> errors) : base(data, statusCode, message, errors)
    {
    }

    public static BaseDataResponse<TData> Success(TData model) => new BaseDataResponse<TData>(model, HttpStatusCode.OK);

    public static BaseDataResponse<TData> Fail(TData model, params ErrorModel[] errors) => new BaseDataResponse<TData>(model, HttpStatusCode.BadRequest, errors);
    public static BaseDataResponse<TData> Fail(TData model, IEnumerable<ErrorModel> errors) => new BaseDataResponse<TData>(model, HttpStatusCode.BadRequest, errors);
    public static BaseDataResponse<TData> Fail(string message) => new BaseDataResponse<TData>(null, HttpStatusCode.BadRequest, message);
    public static BaseDataResponse<TData> Fail(string message, params ErrorModel[] errors) => new BaseDataResponse<TData>(null, HttpStatusCode.BadRequest, message, errors);
    public static BaseDataResponse<TData> Fail(string message, IEnumerable<ErrorModel> errors) => new BaseDataResponse<TData>(null, HttpStatusCode.BadRequest, message, errors);
    public static BaseDataResponse<TData> NotFound(string message) => new BaseDataResponse<TData>(null, HttpStatusCode.NotFound, message);
    public static BaseDataResponse<TData> NotFound() => new BaseDataResponse<TData>(null, HttpStatusCode.NotFound);
    public static BaseDataResponse<TData> Unauthorized(TData model, params ErrorModel[] errors) => new BaseDataResponse<TData>(model, HttpStatusCode.Unauthorized, errors);
    public static BaseDataResponse<TData> Unauthorized(string message) => new BaseDataResponse<TData>(null, HttpStatusCode.Unauthorized, message);
    public static BaseDataResponse<TData> BadRequest(string message) => new BaseDataResponse<TData>(null, HttpStatusCode.BadRequest, message);
    public static BaseDataResponse<TData> Forbidden(string message) => new BaseDataResponse<TData>(null, HttpStatusCode.Forbidden, message);
}

public class BaseDataResponse<TData, TMessage> : BaseResponse<TMessage>
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TData Data { get; set; }
    /// <summary>
    /// Creates a BaseDataResponse without Data. Only for error message usage.
    /// </summary>
    /// <param name="statusCode"></param>
    /// <param name="message">Messsage.</param>
    /// <returns>BaseDataResponse</returns>
    public BaseDataResponse(HttpStatusCode statusCode, string message) : base(statusCode, message)
    {
    }
    public BaseDataResponse(TData data, HttpStatusCode statusCode, string message) : base(statusCode, message)
    {
        Data = data;
    }
    /// <summary>
    /// Creates a BaseDataResponse .
    /// </summary>
    /// <param name="data">TModel value.</param>
    /// <param name="statusCode"></param>
    /// <param name="errors">TMessage messsages.</param>
    /// <returns>BaseDataResponse</returns>
    public BaseDataResponse(TData data, HttpStatusCode statusCode, params TMessage[] errors) : base(statusCode, errors)
    {
        Data = data;
    }
    public BaseDataResponse(TData data, HttpStatusCode statusCode, IEnumerable<TMessage> errors) : base(statusCode, errors)
    {
        Data = data;
    }
    public BaseDataResponse(TData data, HttpStatusCode statusCode, string message, params TMessage[] errors) : base(statusCode, message, errors)
    {
        Data = data;
    }
    public BaseDataResponse(TData data, HttpStatusCode statusCode, string message, IEnumerable<TMessage> errors) : base(statusCode, message, errors)
    {
        Data = data;
    }

}
