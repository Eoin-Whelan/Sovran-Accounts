using Accounts.Data.Contracts;
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
        IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateValidator"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="catalogProxy">The catalog proxy.</param>
        public UpdateValidator(ISovranLogger logger, ICatalogProxy catalogProxy, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _catalogProxy = catalogProxy;
            _unitOfWork = unitOfWork;
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


                // We attempt to update the RDS entry initially.
                var sqlResult = _unitOfWork.Accounts.UpdateAsync(update).Result;
                // If the update was unsuccesful, we kill the operation altogether, passing back an incomplete result.
                if(sqlResult != 1)
                {
                    return Task.FromException(new Exception("Error encountered during update flow."));
                }
                else
                {
                    // If RDS is successful, we build a list of update details for the catalog service, send it asynchronously and proceed.
                    Dictionary<string, string> newDetails = new Dictionary<string, string>();
                    newDetails.Add("email", update.SupportEmail);
                    _catalogProxy.UpdateMerchantAsync(update.Username, newDetails);
                    return Task.CompletedTask;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Issue communication with catalog proxy" + ex.Message);
                return Task.FromException(ex);
            }
        }
    }
}
