using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PharmAssist.Core.Entities.Identity;
using PharmAssist.Repository.Services;



[ApiController]
[Route("api/[controller]")]
public class OtpController : ControllerBase
{
    private readonly OtpService _otpService;
	private readonly UserManager<AppUser> _userManager;
	private readonly ITokenService _tokenService;



	public OtpController(OtpService otpService,
		                 UserManager<AppUser> userManager, 
						 ITokenService tokenService)
    {
        _otpService = otpService;
		_userManager = userManager;
		_tokenService = tokenService;
    }

    [HttpPost("Send")]
    public async Task<IActionResult> SendOtp([FromBody] string email)
    {
        await _otpService.SendOtpAsync(email);
        return Ok("Otp sent");
    }


	[HttpPost("VerifyOtp")]
	public async Task<IActionResult> VerifyOtp([FromBody] OtpVerifyRequest request)
	{
		var (isValid, errorMessage) = await _otpService.VerifyOtpAsync(request.Email, request.Code);
		if (!isValid)
			return BadRequest(new { success = false, message = errorMessage });

		try
		{
			var user = await _userManager.FindByEmailAsync(request.Email);
			if (user == null)
				return BadRequest(new { success = false, message = "User not found." });

			user.EmailConfirmed = true;
			var updateResult = await _userManager.UpdateAsync(user);
			if (!updateResult.Succeeded)
				return BadRequest(new { success = false, message = "User update failed" });

			var token = await _tokenService.CreateTokenAsync(user, _userManager);

			return Ok(new { message = "OTP verified, app access granted.", success = true, token });
		}
		catch (Exception ex)
		{
			return BadRequest(new { success = false, message = "An error occurred while verifying the OTP." });
		}
		
	}
}

public class OtpVerifyRequest
{
    public string Email { get; set; }
    public string Code { get; set; }
}
