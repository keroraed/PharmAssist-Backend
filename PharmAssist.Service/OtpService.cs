using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmAssist.Core.Entities.OTP;
using PharmAssist.Repository.Identity;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using PharmAssist.Service;
using Microsoft.AspNetCore.Identity;
using PharmAssist.Core.Entities.Identity;



public class OtpService
{
    private readonly AppIdentityDbContext _context;
    private readonly OtpConfiguration _otpConfig;
    private readonly EmailService _emailService;
	private readonly UserManager<AppUser> _userManager;


	



	public OtpService(AppIdentityDbContext context, IOptions<OtpConfiguration> otpConfig, EmailService emailService,UserManager<AppUser> userManager)
    {
        _context = context;
        _otpConfig = otpConfig.Value;
        _emailService = emailService;
        _userManager = userManager;
    }

	public async Task SendOtpAsync(string email)
	{

		var code = _emailService.GenerateOtp(); // use existing logic

		var entry = new OtpEntry
		{
			Email = email,
			Code = code,
			ExpiresAt = DateTime.Now.AddMinutes(_otpConfig.ExpiryMinutes),
			IsUsed = false,
			CreatedAt = DateTime.Now
		};

		_context.OtpEntries.Add(entry);
		await _context.SaveChangesAsync();

		await _emailService.SendOtpEmailAsync(email, code);
	}


	public async Task<(bool IsValid, string? ErrorMessage)> VerifyOtpAsync(string email, string code)
	{
		try
		{
			var entry = await _context.OtpEntries
			.Where(x => x.Email == email && x.Code == code && !x.IsUsed)
			.OrderByDescending(x => x.CreatedAt)
			.FirstOrDefaultAsync();

			if (entry == null)
				return (false, "Invalid OTP. Please check your code and try again.");

			if (entry.ExpiresAt < DateTime.Now)
				return (false, "OTP has expired. Please request a new OTP.");

			entry.IsUsed = true;


			await _context.SaveChangesAsync();
			return (true, null);
		}
		catch (Exception ex)
		{
			Console.WriteLine("Exception: " + ex);
			return (false, "An error occurred while verifying the OTP.");
		}

	}
}
