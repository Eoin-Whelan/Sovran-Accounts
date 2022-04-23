using Accounts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Business.Repository
{
    /// <summary>
    /// Unused AccountsContext class for unimplemented ECF class ECFRepo.
    /// </summary>
    public class AccountsContext : DbContext
    {

        public AccountsContext(DbContextOptions<AccountsContext> options) : base(options)
        {
        }
        public DbSet<MerchantAccount> Merchants { get; set; }

        /// <summary>
        /// Method has been made unusable in order to alleviate dev time. In the event
        /// the context class was unconfigured, it would configure itself by directly accessing
        /// an IConfiguration instance that was later removed from this class.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    optionsBuilder.UseMySQL(
            //        "null");
            //}
            //base.OnConfiguring(optionsBuilder);
        }
    }
}
