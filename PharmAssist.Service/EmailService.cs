using MimeKit;
using PharmAssist.Core.Entities.Email;
using PharmAssist.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PharmAssist.Core.Entities.OTP;
using System.Security.Cryptography;

namespace PharmAssist.Service
{
    public class EmailService : IEmailService
	{
		private readonly EmailConfig _emailConfig;
		private readonly OtpConfiguration _otpConfig;

        public EmailService(IOptions<EmailConfig> emailConfig,IOptions<OtpConfiguration> otpConfig)
        {
			_emailConfig = emailConfig.Value;
			_otpConfig=otpConfig.Value;
		}
        public string GenerateOtp()
        {
            int otp = RandomNumberGenerator.GetInt32(0, (int)Math.Pow(10, _otpConfig.Length));
            return otp.ToString($"D{_otpConfig.Length}");
        }
        public async Task SendOtpEmailAsync(string toEmail, string otpCode)
        {
            var message = new Message(
                new[] { toEmail },
<<<<<<< HEAD
                "Your OTP Code",
                $"Please use the OTP code below to verify your identity.",
=======
                "Your Otp Code",
                $"",
>>>>>>> 0741810 (Forgot password)
                otpCode
            );

            await SendEmailAsync(message);
        }


        public async Task SendEmailAsync(Message message)
		{
			var emailMessage = CreateEmailMessage(message);

			await Send(emailMessage);
		}
		private MimeMessage CreateEmailMessage(Message message)
		{
			var emailMessage = new MimeMessage();
			if (string.IsNullOrEmpty(_emailConfig.Email))
			{
				throw new ArgumentException("Email configuration is missing or empty", nameof(_emailConfig.Email));
			}
			emailMessage.From.Add(new MailboxAddress("PharmAssist Support", _emailConfig.Email));
			emailMessage.To.AddRange(message.To);
			emailMessage.Subject = message.Subject;
			var bodyContent = string.IsNullOrEmpty(message.Otp)
			? message.Content
			: $"{message.Content}\nYour Otp is : {message.Otp}. It will expire in {_otpConfig.ExpiryMinutes} minutes.";

			emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = bodyContent };

			return emailMessage;
		}
		private async Task Send(MimeMessage emailMessage)
		{
			using var client = new SmtpClient();
			try
			{

				await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, MailKit.Security.SecureSocketOptions.StartTls);
				await client.AuthenticateAsync(_emailConfig.Email, _emailConfig.Password);
				await client.SendAsync(emailMessage);
			}
			catch(Exception ex)
			{
				Console.WriteLine($"Exception is : {ex}");
			}
			finally
			{
				await client.DisconnectAsync(true);
				client.Dispose();
			}
		}
	}
}
