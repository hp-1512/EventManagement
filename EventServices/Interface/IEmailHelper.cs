using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventServices.Interface
{
    public interface IEmailHelper
    {
         bool SendEmail(string userEmail, string confirmationLink);
    }
}
