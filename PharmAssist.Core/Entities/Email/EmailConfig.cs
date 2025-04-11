using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmAssist.Core.Entities.Email
{
    public class EmailConfig
    {
        public string From { get; set; } = null;
        public string Email { get; set; } = null;
        public string Password { get; set; } = null;
        public string SmtpServer { get; set; } = null;
        public int Port { get; set; }
    }
}
