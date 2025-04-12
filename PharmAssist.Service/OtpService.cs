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

public class OtpService
{
    private readonly AppIdentityDbContext _context;
    private readonly OtpConfiguration _otpConfig;
    private readonly EmailService _emailService;

    public OtpService(AppIdentityDbContext context, IOptions<OtpConfiguration> otpConfig, EmailService emailService)
    {
        _context = context;
        _otpConfig = otpConfig.Value;
        _emailService = emailService;
    }

    public async Task SendOtpAsync(string email)
    {
        var code = _emailService.GenerateOtp(); // use existing logic

        var entry = new OtpEntry
        {
            Email = email,
            Code = code,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_otpConfig.ExpiryMinutes),
            IsUsed = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.OtpEntries.Add(entry);
        await _context.SaveChangesAsync();

        await _emailService.SendOtpEmailAsync(email, code);
    }

    public async Task<bool> VerifyOtpAsync(string email, string code)
    {
        var entry = await _context.OtpEntries
            .Where(x => x.Email == email && x.Code == code && !x.IsUsed)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();

        if (entry == null || entry.ExpiresAt < DateTime.UtcNow)
            return false;

        entry.IsUsed = true;
        await _context.SaveChangesAsync();
        return true;
    }
}
