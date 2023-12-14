using System.Security.Claims;
using Infrastructure.Entities;

namespace Infrastructure.Providers;

//TODO More info about user
public class UserProvider
{
    public ApplicationUser CurrentUser { get; set; }
    public ClaimsPrincipal ContextUser { get; set; }

    public void Initialise(ApplicationUser user, ClaimsPrincipal contextUser)
    {
        CurrentUser = user;
        ContextUser = contextUser;
    }

}
