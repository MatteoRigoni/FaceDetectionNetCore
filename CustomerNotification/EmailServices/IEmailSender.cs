using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmailServices
{
    public interface IEmailSender
    {
        Task SendEmailAsync(Message maessage);
    }
}
