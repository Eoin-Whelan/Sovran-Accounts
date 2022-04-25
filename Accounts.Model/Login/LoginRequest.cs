using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Model.Login
{
    /// <summary>
    /// Main container class for a given login request.
    /// </summary>
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
