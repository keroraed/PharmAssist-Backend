using PharmAssist.Core.Entities.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmAssist.Core.Services
{
	public interface IEmailService
	{
		Task SendEmailAsync(Message message) => throw new NotImplementedException();
		string GenerateOtp();
	}
}
