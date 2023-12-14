using Core.Model.ErrorModels; 
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Core.Model.ResponseModels;
namespace WebApi.Controllers.Common; 
[ApiController]
[Route("api/[controller]/[action]")]
public abstract class ApiController : ControllerBase
{

    protected new BaseResponse Response(BaseResponse responseModel)
    {
        ResponseMetaHandler(responseModel.Errors, responseModel.StatusCode);

        return responseModel;
    }
    protected new BaseDataResponse<T> Response<T>(BaseDataResponse<T> dataResponseModel) where T : class
    {
        ResponseMetaHandler(dataResponseModel.Errors, dataResponseModel.StatusCode);

        return dataResponseModel;

    }

    private void ResponseMetaHandler(ResponseError<ErrorModel> responseError, HttpStatusCode statusCode)
    {
        HttpContext.Response.StatusCode = (int)statusCode;

        if (statusCode != HttpStatusCode.OK) AddModelStateErrors(responseError);
    }

    protected void AddModelStateErrors(ResponseError<ErrorModel> responseError)
    {
        if (responseError == null) return;
        if (responseError.Fields == null)
        {
            responseError.Fields = new List<ErrorModel>();
        }

        foreach (var error in ModelState.Values)
        {
            foreach (var item in error.Errors)
            {
                responseError.Fields.Add(new ErrorModel()
                {
                    Description = item.ErrorMessage
                });
            }
        }
    }


}
