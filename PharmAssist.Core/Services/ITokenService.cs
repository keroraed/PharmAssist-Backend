using Microsoft.AspNetCore.Identity;
using PharmAssist.Core.Entities.Identity;


namespace PharmAssist.Repository.Services
{
	public interface ITokenService
	{
		Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager);
	}
}
