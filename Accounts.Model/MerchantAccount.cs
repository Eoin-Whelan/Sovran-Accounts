using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Model
{

    public class MerchantAccount
    {
        [Key]
        public int MerchantId { get; set; }

        [Required(ErrorMessage = "Missing information: Username")]
        public string Username { get; set; }

        public string StripeId { get; set; }

        public string CatalogId { get; set; }

        [Required(ErrorMessage = "Missing information: Username")]
        public string MerchantEmail { get; set; }

        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Missing information: Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Missing information: First Name")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Missing information: Surname")]
        public string Surname { get; set; }
        
        [Required(ErrorMessage = "Missing information: DoB")]
        public string DateOfBirth { get; set; }
        
        [Required(ErrorMessage = "Missing information: Address Line 1")]
        public string MerchantAddressLineOne { get; set; }
        
        public string MerchantAddressLineTwo { get; set; }
        
        public string MerchantAddressLineThree { get; set; }

        [Required(ErrorMessage = "Missing information: County")]
        public string County { get; set; }

        public string Postcode { get; set; }

        public string SupportPhone { get; set; }

        [Required(ErrorMessage = "Missing information: Customer Contact E-Mail")]
        public string SupportEmail { get; set; }

        public string ProfileImg { get; set; }
    }
}
