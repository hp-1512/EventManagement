using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Entities.Models
{
    public class RegistrationStatus
    {
        public long UserId { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
    }
}
