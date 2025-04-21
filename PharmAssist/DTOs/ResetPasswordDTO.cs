using System.ComponentModel.DataAnnotations;

namespace PharmAssist.DTOs
{
	public class ResetPasswordDTO
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[RegularExpression("(?=^.{6,10}$)(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&amp;*()_+]).*$",
			ErrorMessage = "Password must contains 1 Uppercase, 1 Lowercase, 1 Digit, 1 Special Character")]
		public string Password { get; set; }

		[Compare("Password", ErrorMessage = "Passwords do not match.")]
		public string ConfirmPassword { get; set; }

		public string Otp { get; set; }
	}
}
