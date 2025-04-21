using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PharmAssist.Core.Entities.OTP
{
    public class OtpEntry
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Email { get; set; }

        [Required]
        public string Code { get; set; }

<<<<<<< HEAD
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
=======
        public DateTime CreatedAt { get; set; } = DateTime.Now;
>>>>>>> 0741810 (Forgot password)

        public DateTime ExpiresAt { get; set; }

        public bool IsUsed { get; set; } = false;
    }
}
