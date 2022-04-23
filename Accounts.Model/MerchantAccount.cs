using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Model
{
    /// <summary>
    /// MerchantAccount is the main class representing a new or existing user.
    /// </summary>
    public class MerchantAccount
    {
        /// <summary>
        /// Gets or sets the merchant identifier.
        /// </summary>
        /// <value>
        /// The unique merchant RDS-level identifier.
        /// </value>
        [Key]
        public int MerchantId { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The unique merchant username.
        /// </value>
        [Required(ErrorMessage = "Missing information: Username")]
        public string? Username { get; set; }
        /// <summary>
        /// Gets or sets the stripe identifier.
        /// </summary>
        /// <value>
        /// The Stripe Payments account ID associated with this merchant.
        /// </value>
        public string? StripeId { get; set; }

        /// <summary>
        /// Gets or sets the merchant email.
        /// </summary>
        /// <value>
        /// Email address associated with a given merchant.
        /// </value>
        [Required(ErrorMessage = "Missing information: Email Address")]
        public string? MerchantEmail { get; set; }
        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>
        /// Phone number associated with a given merchant.
        /// </value>
        public string? PhoneNumber { get; set; }
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// Password associated with accessing this account.
        /// </value>
        [Required(ErrorMessage = "Missing information: Password")]
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The merchant's first name.
        /// </value>
        [Required(ErrorMessage = "Missing information: First Name")]
        public string? FirstName { get; set; }
        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        /// <value>
        /// The merchant's surname.
        /// </value>
        [Required(ErrorMessage = "Missing information: Surname")]
        public string? Surname { get; set; }

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        /// <value>
        /// The merchant's date of birth. Currently passed as string (No age validation occurs as of 23/04/22)
        /// </value>
        [Required(ErrorMessage = "Missing information: DoB")]
        public string? DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the merchant address line one.
        /// </summary>
        /// <value>
        /// Address Line 1 of a given merchant.
        /// </value>
        [Required(ErrorMessage = "Missing information: Address Line 1")]
        public string? MerchantAddressLineOne { get; set; }
        /// <summary>
        /// Gets or sets the merchant address line two.
        /// </summary>
        /// <value>
        /// Address Line 2 of a given merchant.
        /// </value>
        public string? MerchantAddressLineTwo { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city/town a merchant is based in.
        /// </value>
        [Required(ErrorMessage = "Missing information: City")]
        public string? City { get; set; }

        /// <summary>
        /// Gets or sets the county.
        /// </summary>
        /// <value>
        /// County in which a merchant is based in. Required as part of address entry.
        /// </value>
        [Required(ErrorMessage = "Missing information: County")]
        public string? County { get; set; }
        /// <summary>
        /// Gets or sets the postcode.
        /// </summary>
        /// <value>
        /// Postcode associated with a given merchant. Required as part of address entry.
        /// </value>
        public string? Postcode { get; set; }
        /// <summary>
        /// Gets or sets the support phone.
        /// </summary>
        /// <value>
        /// Support phone number for a given merchant. Optional.
        /// </value>
        public string? SupportPhone { get; set; }

        /// <summary>
        /// Gets or sets the support email.
        /// </summary>
        /// <value>
        /// The support contact e-mail. Necessary for customer support.
        /// </value>
        [Required(ErrorMessage = "Missing information: Customer Contact E-Mail")]
        public string? SupportEmail { get; set; }
        /// <summary>
        /// Gets or sets the profile img.
        /// </summary>
        /// <value>
        /// The profile img URL. Generated with cloudinary image handler.
        /// </value>
        public string? ProfileImg { get; set; }
    }
}