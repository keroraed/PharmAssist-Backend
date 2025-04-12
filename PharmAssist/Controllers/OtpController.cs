using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class OtpController : ControllerBase
{
    private readonly OtpService _otpService;

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
}

public class OtpVerifyRequest
{
    public string Email { get; set; }
    public string Code { get; set; }
}
