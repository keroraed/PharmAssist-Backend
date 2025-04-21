using System.ComponentModel.DataAnnotations;

namespace PharmAssist.DTOs
{
	public class ForgotPasswordDTO
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}
