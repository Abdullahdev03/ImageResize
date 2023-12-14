using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Model.ResponseModels;
using Core.ViewModels.Account;
using Infrastructure.Data.Contracts;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Core.Services;

public class AccountService
{
     private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public AccountService(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }


    public async Task<BaseDataResponse<TokenDto>> Login(LogInDto login)
    {
        var user = await _unitOfWork.Users
            .FirstOrDefaultAsync(x => x.UserName!.ToLower() == login.UserName.ToLower());

        if (user == null)
            return BaseDataResponse<TokenDto>.Unauthorized("Вы не зарегистрированы");
        var check = await _userManager.CheckPasswordAsync(user, login.Password);

        return !check
            ? BaseDataResponse<TokenDto>.BadRequest("Неверный пароли или имя пользователя")
            : BaseDataResponse<TokenDto>.Success(await GenerateJwtToken(user));
    }


    private async Task<TokenDto> GenerateJwtToken(ApplicationUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
        var claims = new List<Claim>
        {
            new Claim("Id", user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.MobilePhone, user.PhoneNumber!)
        };

        var roles = await _userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(1),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);
        return new TokenDto(tokenString);
    }
    
    
    public async Task<ApplicationUser> GetUserAsync(ClaimsPrincipal userPrincipal)
    {
        if (userPrincipal == null) return null;
        int.TryParse(userPrincipal.Claims.FirstOrDefault(c => c.Type == "Id")?.Value, out int id);

        
        var user = await _unitOfWork.Users.GetDbSet().FirstOrDefaultAsync(p => p.Id == id); // await _userManager.GetUserAsync(userPrincipal);
        return user;
    }
}