namespace Core.Model.ResponseModels;

public class ResponseError<TError>
{
    public ResponseError(IEnumerable<TError> errors)
    {
        Fields = errors?.ToList() ?? new List<TError>();
    }
    public ResponseError(string message)
    {
        Message = message;
        Fields = new List<TError>();
    }
    public ResponseError(string message, IEnumerable<TError> errors)
    {
        Message = message;
        Fields = errors?.ToList() ?? new List<TError>();

    }

    public IList<TError> Fields { get; set; }
    public string Message { get; set; }

}
