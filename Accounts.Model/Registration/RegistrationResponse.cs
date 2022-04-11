using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Model.Registration
{
    public class RegistrationResponse
    {
        public bool isRegistered { get; set; } = false;
        public string? sessionId { get; set; } = null;
    }
}
