using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Model.Registration
{
    /// <summary>
    /// Model for returned object to clientside application.
    /// Contains 
    /// - stripeOnboardingURL (Used in registration flow.)
    /// - Errors encountered
    /// - Result (used in redirect option for Stripe onboarding)
    /// </summary>
    public class RegistrationResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether registration flow was successful <see cref="RegistrationResponse"/> is result.
        /// </summary>
        /// <value>
        ///   <c>true</c> if result; otherwise, <c>false</c>.
        /// </value>
        public bool result { get; set; } = false;
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// Unique user ID pulled from RDS (Unused)
        /// </value>
        public int userId { get; set; }
        /// <summary>
        /// Gets or sets the stripe onboarding URL.
        /// </summary>
        /// <value>
        /// The stripe on boarding URL. Passed back to client as final step of registration.
        /// </value>
        public string? stripeOnBoardingUrl { get; set; }
        /// <summary>
        /// Gets or sets the error MSG.
        /// </summary>
        /// <value>
        /// Indicates if error occured during registation flow.
        /// </value>
        public string? errorMsg { get; set; }
    }
}
