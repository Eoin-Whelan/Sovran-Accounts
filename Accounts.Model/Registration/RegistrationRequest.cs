using Accounts.ServiceClients.Catalog.ApiProxy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Model.Registration
{
    public class RegistrationRequest
    {
        public MerchantAccount NewAccount { get; set; }
        public CatalogEntry NewCatalog { get; set; }
    }
}
