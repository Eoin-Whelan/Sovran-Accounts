using Accounts.Model;
using Accounts.ServiceClients.Catalog.ApiProxy;
using Sovran.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Business.Update
{
    /// <summary>
    /// Handler class for perfoming account update. Communicates with catalog proxy class.
    /// </summary>
    /// <seealso cref="Accounts.Business.Update.IUpdateValidator" />
    public class UpdateValidator : IUpdateValidator
    {
        ISovranLogger _logger;
        ICatalogProxy _catalogProxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateValidator"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="catalogProxy">The catalog proxy.</param>
        public UpdateValidator(ISovranLogger logger, ICatalogProxy catalogProxy)
        {
            _logger = logger;
            _catalogProxy = catalogProxy;
        }
        /// <summary>
        /// Updates a given merchant's catalog account details using Catalog Proxy class.
        /// </summary>
        /// <param name="update">The update.</param>
        /// <returns>Void</returns>
        public Task UpdateAccount(MerchantAccount update)
        {
            try
            {
                _logger.LogActivity("Calling CatalogProxy. Update Merchant call for for: " +update.Username);
                Dictionary<string, string> newDetails = new Dictionary<string, string>();
                newDetails.Add("email", update.SupportEmail);
                _catalogProxy.CatalogUpdateMerchantAsync(update.Username, newDetails);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError("Issue communication with catalog proxy" + ex.Message);
                return Task.FromException(ex);
            }
        }
    }
}
