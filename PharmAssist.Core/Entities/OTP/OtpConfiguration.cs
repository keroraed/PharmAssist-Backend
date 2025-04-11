using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmAssist.Core.Entities.OTP
{
	public class OtpConfiguration
	{
		public int Length { get; set; }
		public int ExpiryMinutes { get; set; }
	}
}
