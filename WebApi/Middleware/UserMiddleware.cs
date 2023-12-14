using Core.Interfaces;
using Core.Services;
using Infrastructure.Providers;

namespace WebApi.Middleware;

public class UserMiddleware
{
    private readonly RequestDelegate next;

    public UserMiddleware(RequestDelegate next)
    {
        this.next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public async Task Invoke(
        HttpContext context,
        UserProvider userProvider,
        AccountService userService
    )
    {
        var user = await userService.GetUserAsync(context.User);

        if (user != null)
        {
            userProvider.Initialise(user, context.User);
        }

        await next(context);
    }
}