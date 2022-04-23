using Accounts.ServiceClients.Catalog.ApiProxy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Model.Registration
{
    /// <summary>
    /// Model for a registration request. Contains a single MerchantAccount and CatalogEntry to pass to SQL and Catalog
    /// service, respectively. 
    /// </summary>
    public class RegistrationRequest
    {
        /// <summary>
        /// Creates new account.
        /// </summary>
        /// <value>
        /// Merchant personal details.
        /// </value>
        [Required]
        public MerchantAccount NewAccount { get; set; }
        /// <summary>
        /// Creates new catalog.
        /// </summary>
        /// <value>
        /// New, public-facing catalog details. During registration, will contain at least one item.
        /// </value>
        [Required]
        public CatalogEntry NewCatalog { get; set; }
    }
}
