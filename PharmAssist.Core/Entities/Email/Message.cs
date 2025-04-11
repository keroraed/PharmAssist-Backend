using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmAssist.Core.Entities.Email
{
	public class Message
	{
			public List<MailboxAddress> To { get; set; }
			public string Subject { get; set; }
			public string Content { get; set; }
		    public string Otp { get; set; }

		public Message(IEnumerable<string> to, string subject, string content, string otp)
		{
			To = new List<MailboxAddress>();
			To.AddRange(to.Select(x => new MailboxAddress("Recipient", x)));
			Subject = subject;
			Content = content;
			Otp = otp;
		}

		public Message(IEnumerable<string> to, string subject, string otp)
		{
			To = new List<MailboxAddress>();
			To.AddRange(to.Select(x => new MailboxAddress("Recipient", x)));
			Subject = subject;
			Otp = otp;
		}
	}
}
