using Microsoft.AspNetCore.Http;
<<<<<<< HEAD
using Microsoft.AspNetCore.Mvc;
=======
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PharmAssist.Core.Entities.Identity;
using PharmAssist.Repository.Services;
>>>>>>> 0741810 (Forgot password)
[ApiController]
[Route("api/[controller]")]
public class OtpController : ControllerBase
{
    private readonly OtpService _otpService;
<<<<<<< HEAD

    public OtpController(OtpService otpService)
    {
        _otpService = otpService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendOtp([FromBody] string email)
    {
        await _otpService.SendOtpAsync(email);
        return Ok("OTP sent");
    }

    [HttpPost("verify")]
    public async Task<IActionResult> VerifyOtp([FromBody] OtpVerifyRequest request)
    {
        var valid = await _otpService.VerifyOtpAsync(request.Email, request.Code);
        return valid ? Ok("OTP verified") : BadRequest("Invalid or expired OTP");
    }
=======
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
		{// Retrieve the user and mark as verified.
			var user = await _userManager.FindByEmailAsync(request.Email);
			if (user == null)
				return BadRequest(new { success = false, message = "User not found." });

			user.EmailConfirmed = true;
			var updateResult = await _userManager.UpdateAsync(user);
			if (!updateResult.Succeeded)
				return BadRequest(new { success = false, message = "User update failed" });

			// Generate the authentication token.
			var token = await _tokenService.CreateTokenAsync(user, _userManager);

			return Ok(new { message = "OTP verified, app access granted.", success = true, token });
		}
		catch (Exception ex)
		{
			return BadRequest(new { success = false, message = "An error occurred while verifying the OTP." });
		}
		
	}
>>>>>>> 0741810 (Forgot password)
}

public class OtpVerifyRequest
{
    public string Email { get; set; }
    public string Code { get; set; }
}
