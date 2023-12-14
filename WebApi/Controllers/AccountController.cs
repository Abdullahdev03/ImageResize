using Core.Model.ResponseModels;
using Core.Services;
using Core.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Common;

namespace WebApi.Controllers;

public class AccountController : ApiController
{
    private readonly AccountService _accountService;

    public AccountController(AccountService accountService)
    {
        _accountService = accountService;
    }
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<BaseDataResponse<TokenDto>> Login([FromBody]LogInDto login)
    {
        var response = await _accountService.Login(login);
        return Response(response);
    }
}