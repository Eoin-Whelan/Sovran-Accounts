using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Model.Registration
{
    public class RegistrationResponse
    {
        public bool result { get; set; } = false;
        public int userId { get; set; }
        public string? stripeOnBoardingUrl { get; set; }
        public string? errorMsg { get; set; }
    }
}
