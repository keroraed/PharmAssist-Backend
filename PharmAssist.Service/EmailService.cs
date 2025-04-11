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
			var random = new Random();
			var otpLength = _otpConfig.Length;
			return new string(Enumerable.Range(0, otpLength).Select(_ => (char)random.Next('0', '9' + 1)).ToArray());
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
			: $"{message.Content}\nYour OTP is : {message.Otp}. It will expire in {_otpConfig.ExpiryMinutes} minutes.";

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
