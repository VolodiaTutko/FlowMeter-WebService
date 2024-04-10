using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IEmailVerificationService
    {
        Task<string> SendVerificationCode(string recipientEmail);
    }
}
